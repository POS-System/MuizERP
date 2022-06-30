using DataAccessLayer;
using DataAccessLayer.Parameters;
using Entities.Company;
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
            var containerDAL = new ContainerDAL(connectionString);

            var companyDAL = containerDAL.CompanyDAL;

            var parametersContainer = new ParametersContainer();
            parametersContainer.Add<Company>("ID", 3);

            var companies = companyDAL.GetItems(parametersContainer);

            var userDAL = containerDAL.UserDAL;

            var newUser = new User
            {
                CompanyID = 2,
                FirstName = "Test 1",
                LastName = "Test 2",
                SecondName = "Test 3",
                BirthDay = new DateTime(2022, 06, 30),
                Email = "new@email.ru",
                GenderID = 0,
                Login = "login",
                Password = "password",
                Phone = "+71236549854",
                Active = true,
                Color = "Color 1",
                Number = 777,
                RoleID = 1,
                ThemeID = 1,
                INN = "123456789878",
                ModifyUserID = 1
            };

            userDAL.SaveItem(newUser);

            //var parametersContainer = new ParametersContainer();
            //var users = userDAL.GetItems(parametersContainer);

            //var sampleEntityDAL = containerDAL.SampleEntityDAL;

            // Тестовая загрузка
            //var sampleEntities = sampleEntityDAL.GetItems();

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

            //sampleEntityDAL.SaveItem(newSampleEntity);

            Console.ReadKey();
        }
    }
}
