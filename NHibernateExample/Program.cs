using System;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Mapping;
using NHibernate;

namespace NHibernateExample
{
    class Program
    {
        public class Employee
        {
            public virtual int Id { get; protected set; }

            public virtual string Name { get; set; }
        }

        public class EmployeeMap : ClassMap<Employee>
        {
            public EmployeeMap()
            {
                Id(x => x.Id);
                Map(x => x.Name);
            }
        }

        static void Main(string[] args)
        {
            var sessionFactory = CreateSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var employee = new Employee {Name = "Empl2"};

                    session.SaveOrUpdate(employee);

                    transaction.Commit();
                }

                using (session.BeginTransaction())
                {
                    var employees = session.CreateCriteria(typeof(Employee))
                        .List<Employee>();

                    foreach (var employee in employees)
                    {
                        Console.WriteLine(employee.Name);
                    }
                }
            }
        }

        private static ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
                .Database(
                    MsSqlConfiguration.MsSql2012.ConnectionString(c => c.FromConnectionStringWithKey("DefaultConnection")))
                .Mappings(
                    m => m.FluentMappings.AddFromAssemblyOf<Employee>())
                .BuildSessionFactory();
        }
    }
}
