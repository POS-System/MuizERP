using DataAccessLayer;
using Entities.Base.Parameters;
using Entities.Exceptions.InnerApplicationExceptions;
using Entities.User;
using System;
using System.Configuration;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionERP"].ConnectionString;
            var dalc = new DALContainer(connectionString);

            //var companyRepository = dalc.CompanyRepository;

            //var parametersContainer = new ParametersContainer();
            //parametersContainer.Add<Company>("ID", 3);

            //var companies = companyRepository.GetItems(parametersContainer);

            //var newCompany = new Company
            //{
            //    ParentID = 4,
            //    Name = "New company 2",
            //    INN = "111111111111",
            //    OrderBy = 204,
            //    ModifyUserID = 1
            //};

            //companyRepository.SaveItem(newCompany);

            var userRepository = dalc.UserRepository;

            var newUser = new User
            {
                CompanyID = 2,
                FirstName = "Тестовое имя 2",
                LastName = "Тестовая фамилия 2",
                SecondName = "Тестовое отчество 2",
                BirthDay = new DateTime(2022, 07, 06),
                Email = "test2@email.ru",
                GenderID = 0,
                Login = "login 2",
                Password = "password ",
                Phone = "+78984562154",
                Active = true,
                Color = "Color 2",
                Number = 777,
                RoleID = 1,
                ThemeID = 1,
                INN = "987456321232",
                ModifyByUserID = 1//,
                //UserRoles = new ObservableCollection<UserRole>
                //{
                //    new UserRole { Role = new Role { ID = 1 } },
                //    new UserRole { Role = new Role { ID = 2 } }
                //}
            };

            userRepository.SaveItem(newUser);
            try
            {
                var users = userRepository.GetItems(new ParametersContainer());
            }
            catch (SqlServerInsertRecordException ex)
            {

            }

            //var sampleEntityRepository = dalc.SampleEntityRepository;

            // Тестовая загрузка
            //var sampleEntities = sampleEntityRepository.GetItems();

            //// Тестовая выгрузка
            //var newSampleEntity = new SampleEntity
            //{
            //    Value = 400,
            //    Description = "Четыреста",
            //    ModifyUserID = 1,

            //    SampleEntityDetailsList = new ObservableCollection<SampleEntityDetails>
            //    {
            //        new SampleEntityDetails()
            //        {
            //            Value = 1,
            //            Description = "Детали 1",
            //            ModifyUserID = 1,
            //        },
            //        new SampleEntityDetails()
            //        {
            //            Value = 2,
            //            Description = "Детали 2",
            //            ModifyUserID = 1,
            //        },
            //        new SampleEntityDetails()
            //        {
            //            Value = 3,
            //            Description = "Детали 3",
            //            ModifyUserID = 1,
            //        }}
            //};

            //sampleEntityRepository.SaveItem(newSampleEntity);

            Console.ReadKey();
        }
    }
}
