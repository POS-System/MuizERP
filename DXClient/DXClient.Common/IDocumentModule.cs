﻿
namespace DXClient.Common
{
    public interface IDocumentModule
    {
        string Caption { get; }
        bool IsActive { get; set; }
    }
}
