﻿using DataAccessLayer.DataReaders;
using DataAccessLayer.Mapping.Interface;
using DataAccessLayer.Repositories.Interfaces;
using DataAccessLayer.Utils;
using Entities;
using Entities.Base;
using Entities.Base.Utils.Interface;
using System.Data.SqlClient;

namespace DataAccessLayer.Repositories
{
    internal sealed class SampleEntityDetailsRepository : ISampleEntityDetailsRepository
    {        
        private readonly DataBaseRepository _dataBaseRepository;
        private readonly IMapper<SqlDataReaderWithSchema, BaseEntity> _baseMapper;

        public SampleEntityDetailsRepository(DataBaseRepository dataBaseRepository, IMapper<SqlDataReaderWithSchema, BaseEntity> baseMapper)
        {
            _dataBaseRepository = dataBaseRepository;
            _baseMapper = baseMapper;
        }

        public EntityCollection<SampleEntityDetails> GetItems(IParametersContainer parameters)
        {
            var result = new EntityCollection<SampleEntityDetails>();

            _dataBaseRepository.ReadCollectionWithSchema<SampleEntityDetails>(
                cmd => cmd.ConfigureParameters(parameters),
                drd =>
                {
                    var item = new SampleEntityDetails();
                    _baseMapper.Map(drd, item);

                    result.Add(item);
                });

            return result;
        }

        public void SaveItem(SampleEntityDetails item, SqlConnection conn)
        {
            _dataBaseRepository.SaveBaseItem(item, conn);
        }
    }
}
