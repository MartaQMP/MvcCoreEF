using Microsoft.EntityFrameworkCore;
using MvcCoreEF.Data;
using MvcCoreEF.Models;

namespace MvcCoreEF.Repositories
{
    public class RepositoryHospital
    {
        private HospitalContext context;

        public RepositoryHospital(HospitalContext context)
        {
            this.context = context;
        }

        public async Task<List<Hospital>> GetHospitalesAsync()
        {
            var consulta = from datos in this.context.Hospitales
                           select datos;
            return await consulta.ToListAsync();
        }

        public async Task<Hospital> FindHospitalByIdAsync(int idHospital)
        {
            var consulta = from datos in this.context.Hospitales
                           where datos.IdHospital == idHospital
                           select datos;
            return await consulta.FirstOrDefaultAsync();
            
        }

        public async Task CreateHospitalAsync(int idHospital, string nombre, string direccion, string telefono, int camas)
        {
            Hospital hospital = new Hospital();
            hospital.IdHospital = idHospital;
            hospital.Nombre = nombre;
            hospital.Direccion = direccion;
            hospital.Telefono = telefono;
            hospital.Camas = camas;
            /* AÑADIMOS NUESTRO OBJETO AL DBSET 
            *  AHORA MISMO ES TEMPORAL ESTA EN LA CONLECCION SALDRA EN LAS CONSULTAS PERO NO ESTA EN LA BBDD */
            await this.context.Hospitales.AddAsync(hospital);
            // GUARDAMOS EN LA BBDD
            await this.context.SaveChangesAsync();
        }

        public async Task DeleteHospitalAsync(int idHospital)
        {
            Hospital hospital = await this.FindHospitalByIdAsync(idHospital);
            // ELIMINAMOS TEMPORALMENTE
            this.context.Hospitales.Remove(hospital);
            await this.context.SaveChangesAsync();
        }

        public async Task UpdateHospitalAsync(int idHospital, string nombre, string direccion, string telefono, int camas)
        {
            Hospital hospital = await this.FindHospitalByIdAsync(idHospital);
            // MODIFICAMOS TODAS SUS PROPIEDADES EXCEPTO SU Key
            hospital.Nombre = nombre;
            hospital.Direccion = direccion;
            hospital.Telefono = telefono;
            hospital.Camas = camas;
            await this.context.SaveChangesAsync();
        }
    }
}
