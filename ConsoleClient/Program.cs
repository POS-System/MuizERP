using DataAccessLayer;
using Entities.SampleEntityN;
using Entities.SampleEntityDetailsN;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Entities.SampleEntity;
using DataAccessLayer.Parameters;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var containerDAL = new ContainerDAL();
            var sampleEntityDAL = containerDAL.SampleEntityDAL;

            SampleEntityFilter sampleEntityFilter = new SampleEntityFilter();
            sampleEntityFilter.ValueMax = 300;
            sampleEntityFilter.ValueMin = 0;
            sampleEntityFilter.UserName = "And";

            ParametersContainer parametersContainer = new ParametersContainer();
            parametersContainer.AddRange(sampleEntityFilter);

            // Тестовая загрузка
            var sampleEntities = sampleEntityDAL.GetItems(parametersContainer);

            // Тестовая выгрузка
            var newSampleEntity = new SampleEntity
            {
                Value = 500,
                Description = "Пятсот",
                ModifyUserID = 1,

                SampleEntityDetailsList = new ObservableCollection<SampleEntityDetails>
                {
                    new SampleEntityDetails()
                    {
                        Value = 1,
                        Description = "Детали 1",
                        ModifyUserID = 1,
                    },
                    new SampleEntityDetails()
                    {
                        Value = 2,
                        Description = "Детали 2",                        
                        ModifyUserID = 1,
                    },
                    new SampleEntityDetails()
                    {
                        Value = 3,
                        Description = "Детали 3",
                        ModifyUserID = 1,
                    }}
            };
            
            sampleEntityDAL.SaveItem(newSampleEntity);

            Console.ReadKey();
        }
    }
}
