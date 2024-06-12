﻿using DAL;
using Model;

namespace Service
{
    // This class was written by Andrew Lia
    public class TableService : BaseService
    {
        private TableDao tableDao = new();
        public event Action TableOccupiedChanged;

        public List<Table> GetAllTables()
        {
            return tableDao.GetAllTables();
        }

        public Table GetTableById(int id)
        {
            return tableDao.GetTableById(id);
        }

        public void UpdateTableStatus(Table table)
        {
            tableDao.UpdateTableStatus(table);
        }

        protected override void CheckForChanges(object sender, EventArgs e)
        {
            //TableOccupiedChanged?.Invoke();
        }

        public bool EqualTableoccupation(Table updatedTable,  Table currentTable)
        {
            return updatedTable.Occupied.Equals(currentTable.Occupied);
        }
    }
}