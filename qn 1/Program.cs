using System;
using System.Data.SqlClient;

// Base Employee class
public abstract class Employee
{
    public int EmployeeID { get; set; }
    public string? Name { get; set; }
    public string? ReportingManager { get; set; }
}

// Payroll Employee
public class PayrollEmployee : Employee
{
    public DateTime JoiningDate { get; set; }
    public int Experience { get; set; }
    public decimal BasicSalary { get; set; }
    public decimal DA { get; set; }
    public decimal HRA { get; set; }
    public decimal PF { get; set; }
    public decimal NetSalary { get; set; }

    public void CalculateNetSalary()
    {
        (DA, HRA, PF) = Experience switch
        {
            > 10 => (BasicSalary * 0.10m, BasicSalary * 0.085m, 6200m),
            > 7 => (BasicSalary * 0.07m, BasicSalary * 0.065m, 4100m),
            > 5 => (BasicSalary * 0.041m, BasicSalary * 0.038m, 1800m),
            _ => (BasicSalary * 0.019m, BasicSalary * 0.020m, 1200m)
        };
        NetSalary = BasicSalary + DA + HRA - PF;
    }
}

// Contract Employee
public class ContractEmployee : Employee
{
    public DateTime ContractDate { get; set; }
    public int Duration { get; set; }
    public decimal Charges { get; set; }
}

public class EmployeeManager
{
    private readonly string _cs = "Server=(localdb)\\MSSQLLocalDB;Database=CompanyDB;Integrated Security=SSPI;";

    private void ExecuteNonQuery(string query, Action<SqlCommand> paramSetter)
    {
        using var conn = new SqlConnection(_cs);
        using var cmd = new SqlCommand(query, conn);
        paramSetter(cmd);
        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public int AddEmployee(string name, string manager, string type)
    {
        using var conn = new SqlConnection(_cs);
        var cmd = new SqlCommand("INSERT INTO Employees (Name, ReportingManager, EmployeeType) VALUES (@n,@m,@t); SELECT SCOPE_IDENTITY();", conn);
        cmd.Parameters.AddWithValue("@n", name);
        cmd.Parameters.AddWithValue("@m", manager);
        cmd.Parameters.AddWithValue("@t", type);
        conn.Open();
        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    public void AddPayrollDetails(PayrollEmployee e) =>
        ExecuteNonQuery("INSERT INTO PayrollEmployees VALUES (@id,@join,@exp,@basic,@da,@hra,@pf,@net)", cmd =>
        {
            cmd.Parameters.AddWithValue("@id", e.EmployeeID);
            cmd.Parameters.AddWithValue("@join", e.JoiningDate);
            cmd.Parameters.AddWithValue("@exp", e.Experience);
            cmd.Parameters.AddWithValue("@basic", e.BasicSalary);
            cmd.Parameters.AddWithValue("@da", e.DA);
            cmd.Parameters.AddWithValue("@hra", e.HRA);
            cmd.Parameters.AddWithValue("@pf", e.PF);
            cmd.Parameters.AddWithValue("@net", e.NetSalary);
        });

    public void AddContractDetails(ContractEmployee e) =>
        ExecuteNonQuery("INSERT INTO ContractEmployees VALUES (@id,@cdate,@dur,@charges)", cmd =>
        {
            cmd.Parameters.AddWithValue("@id", e.EmployeeID);
            cmd.Parameters.AddWithValue("@cdate", e.ContractDate);
            cmd.Parameters.AddWithValue("@dur", e.Duration);
            cmd.Parameters.AddWithValue("@charges", e.Charges);
        });

    public void DisplayAllEmployees()
    {
        using var conn = new SqlConnection(_cs);
        conn.Open();

        void PrintEmployees(string title, string query)
        {
            Console.WriteLine($"\n--- {title} ---");
            using var cmd = new SqlCommand(query, conn);
            using var r = cmd.ExecuteReader();
            while (r.Read())
                Console.WriteLine(string.Join(", ", r));
        }

        PrintEmployees("Payroll Employees", "SELECT e.EmployeeID,e.Name,e.ReportingManager,p.JoiningDate,p.Experience,p.BasicSalary,p.NetSalary FROM Employees e JOIN PayrollEmployees p ON e.EmployeeID=p.EmployeeID");
        PrintEmployees("Contract Employees", "SELECT e.EmployeeID,e.Name,e.ReportingManager,c.ContractDate,c.Duration,c.Charges FROM Employees e JOIN ContractEmployees c ON e.EmployeeID=c.EmployeeID");
    }

    public int GetTotalEmployeeCount()
    {
        using var conn = new SqlConnection(_cs);
        var cmd = new SqlCommand("SELECT COUNT(*) FROM Employees", conn);
        conn.Open();
        return Convert.ToInt32(cmd.ExecuteScalar());
    }
}

public class Program
{
    public static void Main()
    {
        var manager = new EmployeeManager();
        string cont = "y";

        while (cont.ToLower() == "y")
        {
            Console.Write("Enter employee type (Payroll/Contract): ");
            string type = Console.ReadLine()!;
            Console.Write("Enter Name: ");
            string name = Console.ReadLine()!;
            Console.Write("Enter Reporting Manager: ");
            string mgr = Console.ReadLine()!;
            int id = manager.AddEmployee(name, mgr, type);

            if (type.Equals("payroll", StringComparison.OrdinalIgnoreCase))
            {
                var p = new PayrollEmployee { EmployeeID = id, Name = name, ReportingManager = mgr };
                Console.Write("Enter Joining Date (YYYY-MM-DD): ");
                p.JoiningDate = DateTime.Parse(Console.ReadLine()!);
                Console.Write("Enter Experience: ");
                p.Experience = int.Parse(Console.ReadLine()!);
                Console.Write("Enter Basic Salary: ");
                p.BasicSalary = decimal.Parse(Console.ReadLine()!);
                p.CalculateNetSalary();
                manager.AddPayrollDetails(p);
            }
            else
            {
                var c = new ContractEmployee { EmployeeID = id, Name = name, ReportingManager = mgr };
                Console.Write("Enter Contract Date (YYYY-MM-DD): ");
                c.ContractDate = DateTime.Parse(Console.ReadLine()!);
                Console.Write("Enter Duration (months): ");
                c.Duration = int.Parse(Console.ReadLine()!);
                Console.Write("Enter Charges: ");
                c.Charges = decimal.Parse(Console.ReadLine()!);
                manager.AddContractDetails(c);
            }

            Console.WriteLine("Employee added successfully!");
            Console.Write("Add another employee? (y/n): ");
            cont = Console.ReadLine()!;
        }

        Console.WriteLine("\nDisplaying all employees:");
        manager.DisplayAllEmployees();
        Console.WriteLine($"\nTotal number of employees: {manager.GetTotalEmployeeCount()}");
    }
}


}
