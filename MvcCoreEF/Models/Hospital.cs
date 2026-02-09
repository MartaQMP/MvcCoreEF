using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcCoreEF.Models
{
    // MEDIANTE DECORACIONES DE MAPPING VAMOS INDICANDO A QUE TABLA/VIEW APUNTA 
    [Table("HOSPITAL")]
    public class Hospital
    {
        [Key]
        [Column("HOSPITAL_COD")]
        public int IdHospital { get; set; }
        [Column("NOMBRE")]
        public string Nombre { get; set; }
        [Column("DIRECCION")]
        public string Direccion { get; set; }
        [Column("TELEFONO")]
        public string Telefono { get; set; }
        [Column("NUM_CAMA")]
        public int Camas { get; set; }
    }
}
