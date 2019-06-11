using System.ComponentModel.DataAnnotations;

namespace Modelo.EntityFramework.MonitorPontofopag
{
    [MetadataType(typeof(JobControlMetaData))]
    public partial class JobControl
    {
        public bool Reprocessar { get; set; }
    }


    public class JobControlMetaData
    {
        
    }
}
