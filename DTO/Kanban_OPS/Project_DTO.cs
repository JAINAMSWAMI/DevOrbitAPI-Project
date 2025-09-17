namespace DevOrbitAPI.DTO.Kanban_OPS
{
    public class Project_DTO
    {
        public string Project_Name { get; set; }
        public string Project_Description { get; set; }
        public string Project_Status { get; set; }
        public string Project_Version { get; set; }

    }
    public class GetProjectDTO
    {
        public int Project_ID { get; set; }
        public string Project_Name { get; set; }
        public string Project_Description { get; set; }
        public string Project_Created_By { get; set; }
        public string Project_Status { get; set; }
        public string Project_Version { get; set; }
        public DateTime Project_Created_At { get; set; }
    }
}
