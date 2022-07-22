using Entities.Base;
using Entities.Base.Attributes;
using Entities.Base.Utils;
using Entities.Base.Utils.Factories;
using Entities.Base.Utils.Providers;
using Entities.Base.Utils.Validators;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace DataAccessLayer.Factories
{
    internal sealed class DataTableFactory : IKeyedFactory<IEntityCollection, IEnumerable<DataRow>, DataTable>
    {
        private readonly IKeyedProvider<string, Type> _columnTypeProvider;
        private readonly IKeyedProvider<PropertyInfo, string> _columnNameProvider;

        public DataTableFactory(
            IKeyedProvider<string, Type> columnTypeProvider,
            IKeyedProvider<PropertyInfo, string> columnNameProvider)
        {
            ArgumentValidator.ValidateThatArgumentNotNull(columnTypeProvider, "columnTypeProvider");
            ArgumentValidator.ValidateThatArgumentNotNull(columnNameProvider, "columnNameProvider");

            _columnTypeProvider = columnTypeProvider;
            _columnNameProvider = columnNameProvider;
        }

        public DataTable Create(IEntityCollection collection, IEnumerable<DataRow> schema)
        {
            var table = new DataTable();
            CreateColumnsBySchema(table, schema);
            FillTable(table, collection);

            return table;
        }

        private void CreateColumnsBySchema(DataTable table, IEnumerable<DataRow> schema)
        {
            foreach (var columnSchema in schema)
            {
                var column = new DataColumn();

                var columnName = columnSchema.ItemArray[3];
                if (columnName != DBNull.Value)
                    column.ColumnName = Convert.ToString(columnName);

                var allowDBNull = columnSchema.ItemArray[6];
                if (allowDBNull != DBNull.Value)
                    column.AllowDBNull = Convert.ToString(allowDBNull) == "YES";

                var dataType = columnSchema.ItemArray[7];
                if (dataType != DBNull.Value)
                    column.DataType = _columnTypeProvider.GetByValue(Convert.ToString(dataType));

                var maxLength = columnSchema.ItemArray[8];
                if (maxLength != DBNull.Value)
                    column.MaxLength = Convert.ToInt32(maxLength);

                table.Columns.Add(column);
            }
        }

        private void FillTable(DataTable table, IEntityCollection collection)
        {
            var properties = GetEntityProperties(collection);

            foreach (var item in collection)
            {
                var row = table.NewRow();

                foreach (var property in properties)
                {
                    var name = _columnNameProvider.GetByValue(property);
                    var value = property.GetValue(item);
                    row[name] = value;
                }

                table.Rows.Add(row);
            }
        }

        private IEnumerable<PropertyInfo> GetEntityProperties(IEntityCollection collection)
        {
            var type = collection.GetType().GetGenericArguments()[0];

            return type.GetCustomPropertiesWithAttribute<SaveParameterAttribute>()
                .Where(p => p.Name != "TimeStamp");
        }
    }
}
