/* http://www.zkea.net/ Copyright 2016 ZKEASOFT http://www.zkea.net/licenses */
using Easy.MetaData;

namespace MvcApplication.Models
{
    [DataConfigure(typeof(ExampleItemMeterData))]
    public class ExampleItem
    {
        public int? ID { get; set; }
        public int? ExampleID { get; set; }
        public string OwnerId { get; set; }
    }

    class ExampleItemMeterData : DataViewMetaData<ExampleItem>
    {

        protected override void DataConfigure()
        {
            DataTable("ExampleItem");
            DataConfig(m => m.ID).AsIncreasePrimaryKey();
        }

        protected override void ViewConfigure()
        {

        }
    }


}