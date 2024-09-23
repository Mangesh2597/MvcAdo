using System.Data.SqlClient;
using MvcAdo.Models;
using System.Data;
using System.Data.SqlTypes;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Identity;

namespace MvcAdo.DAL 
{
    public class Employee_DAL
    {
        SqlConnection _connection=null;
        SqlCommand _command =null;
        public static IConfiguration Configuration {get;set;}

        private string GetConnectionString()
        {
            var builder =new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            Configuration=builder.Build();
            return Configuration.GetConnectionString("DefaultConnection");
        }

        public List<Employee> GetAll()
        {
              List<Employee> employees = new List<Employee>();

            using (_connection = new SqlConnection(GetConnectionString()))
            {
                _command = _connection.CreateCommand();
                _command.CommandType =  CommandType.StoredProcedure;
                _command.CommandText= "usp_GetAllEmployees";
                _command.Connection.Open();
                SqlDataReader dr = _command.ExecuteReader();

                while (dr.Read())
                {
                    Employee emp = new Employee();
                    emp.Id=Convert.ToInt32(dr["Id"]);
                    emp.FirstName=dr["FirstName"].ToString();
                    emp.LastName=dr["LastName"].ToString();
                    emp.DateOfBirth=Convert.ToDateTime(dr["DateOfBirth"]);
                    emp.Email=dr["Email"].ToString();
                    emp.Salary=Convert.ToDouble(dr["Salary"]);

                    employees.Add(emp);

                }
                _connection.Close();
            } 
           return employees;
        } 
        public bool Create(Employee model)
        {
                int id=0;

            using(_connection= new SqlConnection(GetConnectionString()))
            {
                _command=_connection.CreateCommand();
                _command.CommandType= CommandType.StoredProcedure;
                _command.CommandText="usp_InsertEmployee";
                _command.Parameters.AddWithValue("@FirstName",model.FirstName);
                _command.Parameters.AddWithValue("@LastName",model.LastName);
                _command.Parameters.AddWithValue("@Date",model.DateOfBirth);
                _command.Parameters.AddWithValue("@EMail",model.Email);
                _command.Parameters.AddWithValue("@Salary",model.Salary);
                _connection.Open();
                id=_command.ExecuteNonQuery();
                _connection.Close();
            }
            return id>0?true:false;
        }

        public Employee GetById(int id)
        {
            Employee emp = new Employee();
            using(_connection = new SqlConnection(GetConnectionString()))
            {
               _command= _connection.CreateCommand();
               _command.CommandType= CommandType.StoredProcedure;
               _command.CommandText="usp_GetEmployeeById";
               _command.Parameters.AddWithValue("@Id",id);
               _connection.Open();
               SqlDataReader dr =_command.ExecuteReader();
               if(dr.Read())
               {
                emp.FirstName= dr["FirstName"].ToString();
                emp.LastName= dr["LastName"].ToString();
                emp.Email= dr["Email"].ToString();
                emp.DateOfBirth =Convert.ToDateTime( dr["DateOfBirth"]);
                emp.Salary =Convert.ToInt32(dr["Salary"]);
               }
               return emp;

            }
            
        }
        public bool updateEmployee(Employee model)
        {
            int id =0;
            using(_connection= new SqlConnection(GetConnectionString()))
            {
                _command= _connection.CreateCommand();
                _command.CommandType=CommandType.StoredProcedure;
                _command.CommandText="usp_updateEmployees";
                _command.Parameters.AddWithValue("@Id",model.Id);
                _command.Parameters.AddWithValue("@FirstName", model.FirstName);
                _command.Parameters.AddWithValue("@LastName", model.LastName);
                _command.Parameters.AddWithValue("@EMail", model.Email);
                _command.Parameters.AddWithValue("@Date", model.DateOfBirth);
                _command.Parameters.AddWithValue("@Salary", model.Salary);
                _connection.Open();
                id=_command.ExecuteNonQuery();
                _connection.Close();
            }
            return id>0?true:false;
        }

        public bool delete(int Id)
        {
            int id=0;
            using(_connection=new SqlConnection(GetConnectionString()))
            {
                _command=_connection.CreateCommand();
                _command.CommandType=CommandType.StoredProcedure;
                _command.CommandText= "usp_deleteEmoloyee";
                _command.Parameters.AddWithValue("@Id",Id);
                _connection.Open();
                id=_command.ExecuteNonQuery();
                _connection.Close();
            }
            return id>0?true:false;
        }
    }
    
}