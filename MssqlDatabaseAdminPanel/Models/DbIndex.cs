using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MssqlDatabaseAdminPanel.Models {
    public class DbIndex {
        public string TableName { get; set; }
        public string EqualityColumns { get; set; }
        public string InequalityColumns { get; set; }
        public string IncludedColumns { get; set; }
        public string Name { get; set; }
    }
}
