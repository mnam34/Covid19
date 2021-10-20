namespace Entities.Enums
{
    /// <summary>
    /// Các hành động của chức năng
    /// </summary>
    public enum ActionEnum
    {
        Read = 1,
        Create = 2,
        Update = 3,
        Delete = 4,
        Confirm = 5,
        Approve = 6,
        Verify = 7,
        Publish = 8,
        NoCheck = -1
    }
    public class ActionType
    {
        public const int Read = 1;
        public const int Create = 2;
        public const int Update = 3;
        public const int Delete = 4;

        public const int Confirm = 5;
        public const int Approve = 6;

        public const int Verify = 7;
        public const int Publish = 8;
        public const int NoCheck = -1;
    }
}
