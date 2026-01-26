using Demo_DI_PropertyInjection.Model;

public class Employee
{
    public int EmployeeId;
    public string EmployeeName;
    private IDepartment _employeeDept;

    public Employee(int id, string name)
    {
        EmployeeId = id;
        EmployeeName = name;
    }

    // --------------------------------------
    // Property Injection
    public IDepartment EmployeeDept
    {
        get
        {
            if (this._employeeDept == null)
                this.EmployeeDept = new Engineering();
            return this._employeeDept;
        }
        set
        {
            if (value == null)
                throw new ArgumentNullException("Null");
            if (this._employeeDept != null)
                throw new InvalidOperationException();
            this._employeeDept = value;
        }
    }
    // end property
    // --------------------------------------

    public override string ToString()
    {
        return $"EmpID:{EmployeeId}, Emp Name:{EmployeeName}, " +
               $"Department:{_employeeDept.DeptName}";
    }
}
// end class
