namespace Demo_DI_MethodInjection.Model
{
    public interface IDepartment
    {
        int DeptId { get; set; }
        string DeptName { get; set; }
    }

    //-----------------------------------------

    public class Department : IDepartment
    {
        public int DeptId { get; set; }
        public string DeptName { get; set; }
    }

    //-----------------------------------------

    public class Engineering : Department
    {
        public Engineering()
        {
            DeptName = "Engineering";
        }
    }

    //-----------------------------------------

    public class Marketing : Department
    {
        public Marketing()
        {
            DeptName = "Marketing";
        }
    }
}
