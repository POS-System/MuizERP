using DataAccessLayer;
using Entities.SampleEntity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var containerDAL = new ContainerDAL();
            var sampleEntityDAL = containerDAL.SampleEntityDAL;

            // Тестовая загрузка
             var sampleEntities = sampleEntityDAL.GetItems();

            // Тестовая выгрузка
            var newSampleEntity = new SampleEntity
            {
                Value = 400,
                Description = "Четыреста",
                LastModifiedDate = DateTime.Now,
                CreatedDate = DateTime.Now,
                LastModifiedByUserID = 1,
                CreatedByUserID = 1,
                AddUserID = 1,

                SampleEntityDetailsList = new ObservableCollection<SampleEntityDetails>
                {
                    new SampleEntityDetails()
                    {
                        Value = 1,
                        Description = "Детали 1",
                        LastModifiedDate = DateTime.Now,
                        CreatedDate = DateTime.Now,
                        LastModifiedByUserID = 1,
                        CreatedByUserID = 1,
                        AddUserID = 1,
                    },
                    new SampleEntityDetails()
                    {
                        Value = 2,
                        Description = "Детали 2",
                        LastModifiedDate = DateTime.Now,
                        CreatedDate = DateTime.Now,
                        LastModifiedByUserID = 1,
                        CreatedByUserID = 1,
                        AddUserID = 1,
                    },
                    new SampleEntityDetails()
                    {
                        Value = 3,
                        Description = "Детали 3",
                        LastModifiedDate = DateTime.Now,
                        CreatedDate = DateTime.Now,
                        LastModifiedByUserID = 1,
                        CreatedByUserID = 1,
                        AddUserID = 1,
                    }}
            };
            
            sampleEntityDAL.SaveItem(newSampleEntity);

            Console.ReadKey();
        }
    }
}
