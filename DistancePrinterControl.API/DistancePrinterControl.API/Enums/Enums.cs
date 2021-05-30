namespace DistancePrinterControl.API.Enums
{
    public class Enums
    {
        public enum UserType
        {
            Guest = 1,
            Student = 2,
            Engineer = 3
        }

        public enum MessengerType
        {
            Telegram = 1,
            Viber = 2,
            Email = 3
        }

        public enum ActionType
        {
            ManageUsers = 1,
            UnlockRoom = 2,
            Manage3DPrinters = 3,
            Use3DPrinters = 4,
            Issue3DPrinters = 5,
            ManageRooms = 6,
            ManageUserRooms = 7,
            ManageComputers = 8,
            IssueComputers = 9,
            IssueIoTSets = 10,
            ManageIoTSets = 11
        }
    }
}