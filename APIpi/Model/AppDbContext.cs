﻿using Microsoft.EntityFrameworkCore;

namespace APIpi.Model
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Locacion> Locaciones { get; set; }
        public DbSet<ServiciosAdicionales> Servicios_Adicionales { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Eventos> Eventos { get; set; }
        public DbSet<Agenda> Agendas { get; set; }
        public DbSet<DetallesServicios> Detalles_Servicios { get; set; }
        public DbSet<Facturas> Facturas { get; set; }
        public DbSet<Bitacora> Bitacora { get; set; }
    }
}
