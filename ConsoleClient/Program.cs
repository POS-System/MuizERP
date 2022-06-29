using DataAccessLayer;
using DataAccessLayer.Parameters;
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

            var userDAL = containerDAL.UserDAL;

            var parametersContainer = new ParametersContainer();
            var users = userDAL.GetItems(parametersContainer);

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
