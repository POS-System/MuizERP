﻿using DataAccessLayer.DataReaders;
using DataAccessLayer.Mapping.Interface;
using DataAccessLayer.Repositories.Interfaces;
using Entities.Base;
using Entities.Base.Utils.Interface;
using Entities.UserSettings;

namespace DataAccessLayer.Repositories
{
    internal class UserSettingsRepository : IUserSettingsRepository
    {
        private readonly DataRepository _dataRepository;
        private readonly IMapper<SqlDataReaderWithSchema, UserSettings> _userSettingsMapper;

        public UserSettingsRepository(
            DataRepository dataRepository,
            IMapper<SqlDataReaderWithSchema, UserSettings> userSettingsMapper)
        {
            _dataRepository = dataRepository;

            _userSettingsMapper = userSettingsMapper;
        }

        public EntityCollection<UserSettings> GetItems(IParametersContainer parameters)
        {
            var result = new EntityCollection<UserSettings>();

            _dataRepository.ReadCollectionWithSchema<UserSettings>(
                cmd => { },
                drd =>
                {
                    var item = new UserSettings();
                    _userSettingsMapper.Map(drd, item);

                    result.Add(item);
                });

            return result;
        }
    }
}
