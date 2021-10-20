var eaId = $('#SelectEpidemicArea').val();
var jsonRoute = '/sm/chart-json3/' + eaId;
$(function () {
    $('#tree')
        .jstree({
            'core': {
                'data': {
                    'url': function (node) {
                        return node.id === '#' ?
                            jsonRoute + '?parent=parent' :
                            jsonRoute + '?parent=' + node.id;
                    },
                    'data': function (node) {
                        return { 'id': node.id };
                    }
                },
                'force_text': true,
                'check_callback': true,
                'themes': {
                    'responsive': false
                }
            },
            "plugins": ["search", "types", "themes", "html_data"]
        })
        .on('changed.jstree', function (e, data) {
            if (data && data.selected && data.selected.length) {
                var parent = data.selected.join(':');
                //reloadPage2(parent);
            }
            else {
                //reloadPage2('parent');
            }
        });
    var to = false;
    $('#tree_q').keyup(function () {
        if (to) { clearTimeout(to); }
        to = setTimeout(function () {
            var v = $('#tree_q').val();
            $('#tree').jstree(true).search(v, false, true);
        }, 250);
    });
});

function loopChart($hierarchy) {
    var $siblings = $hierarchy.children('.nodes').children('.hierarchy');
    if ($siblings.length) {
        $siblings.filter(':not(.hidden)').first().addClass('first-shown')
            .end().last().addClass('last-shown');
    }
    $siblings.each(function (index, sibling) {
        loopChart($(sibling));
    });
}

function filterNodes(keyWord) {
    if (!keyWord.length) {
        window.alert('Vui lòng nhập nội dung cần tìm!');
        return;
    } else {
        var $chart = $('.orgchart');
        // disalbe the expand/collapse feture
        $chart.addClass('noncollapsable');
        // distinguish the matched nodes and the unmatched nodes according to the given key word
        $chart.find('.node').filter(function (index, node) {
            return $(node).text().toLowerCase().indexOf(keyWord) > -1;
        }).addClass('matched')
            .closest('.hierarchy').parents('.hierarchy').children('.node').addClass('retained');
        // hide the unmatched nodes
        $chart.find('.matched,.retained').each(function (index, node) {
            $(node).removeClass('slide-up')
                .closest('.nodes').removeClass('hidden')
                .siblings('.hierarchy').removeClass('isChildrenCollapsed');
            var $unmatched = $(node).closest('.hierarchy').siblings().find('.node:first:not(.matched,.retained)')
                .closest('.hierarchy').addClass('hidden');
        });
        // hide the redundant descendant nodes of the matched nodes
        $chart.find('.matched').each(function (index, node) {
            if (!$(node).siblings('.nodes').find('.matched').length) {
                $(node).siblings('.nodes').addClass('hidden')
                    .parent().addClass('isChildrenCollapsed');
            }
        });
        // loop chart and adjust lines
        loopChart($chart.find('.hierarchy:first'));
    }
}

function clearFilterResult() {
    $('.orgchart').removeClass('noncollapsable')
        .find('.node').removeClass('matched retained')
        .end().find('.hidden, .isChildrenCollapsed, .first-shown, .last-shown').removeClass('hidden isChildrenCollapsed first-shown last-shown')
        .end().find('.slide-up, .slide-left, .slide-right').removeClass('slide-up slide-right slide-left');
    $('#key-word').val('');
}


function clickExportButton() {
    $(".oc-export-btn").click();
}
$(document).ready(function () {
    var oc = $('#chart-container').orgchart({
        'pan': true,
        'zoom': true,
        'data': '',
        'nodeContent': 'title',
        'nodeID': 'id',
        'exportButton': true,
        'exportFilename': 'danh-sach-f-case',
        'direction': 'l2r'
        /*'exportFileextension': 'pdf',*/
        //'verticalLevel': 2,
        //'visibleLevel': 4,
    });
    oc.$chartContainer.on('touchmove', function (event) {
        event.preventDefault();
    });
    $('#btn-filter-node').on('click', function () {
        filterNodes($('#key-word').val());
    });

    $('#btn-cancel').on('click', function () {
        clearFilterResult();
    });

    $('#key-word').on('keyup', function (event) {
        if (event.which === 13) {
            filterNodes(this.value);
        } else if (event.which === 8 && this.value.length === 0) {
            clearFilterResult();
        }
    });
   
    oc.init({ 'data': '/sm/chart-json/' + eaId });
    $(document).on('change', '#SelectEpidemicArea',function (argument) {
        oc.init({ 'data': '/sm/chart-json/' + $(this).val() });
        jsonRoute = '/sm/chart-json3/' + $(this).val();
        //$('#tree').jstree(true).settings.core.data = jsonRoute;
        //$('#tree').jstree(true).settings.core.data = { 'url': jsonRoute };
        $('#tree').jstree(true).refresh();
    });
});