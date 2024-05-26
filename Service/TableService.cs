﻿using DAL;
using Model;

namespace Service
{
    // This class was written by Andrew Lia
    public class TableService
    {
        private TableDao tableDao;

        public List<Table> GetAllTables()
        {
            return tableDao.GetAllTables();
        }

        public Table GetTableById(uint id)
        {
            return tableDao.GetTableById(id);
        }
    }
}
