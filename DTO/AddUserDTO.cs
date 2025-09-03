namespace DevOrbitAPI.DTO
{
    public class AddUserDTO
    {
        public string User_FullName { get; set; }
        public string UserName { get; set; }
        public string User_Email { get; set; }
        public string User_Assigned_Pass { get; set; }
        public string User_Role { get; set; }
        public long  User_Phone_No { get; set; }
        public string User_Designation { get; set; }
        public string User_Department { get; set; }
        public DateTime User_LastLogin { get; set; }
    }

}
