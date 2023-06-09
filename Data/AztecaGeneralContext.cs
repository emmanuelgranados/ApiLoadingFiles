﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using apiOperadores.Models;

namespace apiOperadores.Data
{
    public partial class AztecaGeneralContext : DbContext
    {
        public AztecaGeneralContext()
        {
        }

        public AztecaGeneralContext(DbContextOptions<AztecaGeneralContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Bitacora> Bitacoras { get; set; }
        public virtual DbSet<Operadore> Operadores { get; set; }
        public virtual DbSet<OperadoresStatus> OperadoresStatuses { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<AppOpAztecaAcceso> AppOpAztecaAccesos { get; set; }
        public virtual DbSet<AppOpAztecaViajes> AppOpAztecaViajes { get; set; }
        public virtual DbSet<ViajeDocumentos> ViajeDocumentos { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bitacora>(entity =>
            {
                entity.ToTable("bitacora", "seguridad");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CamposAfectados)
                    .HasColumnType("text")
                    .HasColumnName("campos_afectados");

                entity.Property(e => e.FechaMovimiento)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_movimiento")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FkUsuarioId).HasColumnName("fk_usuario_id");

                entity.Property(e => e.TablaAfectada)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("tabla_afectada");

                entity.Property(e => e.TipoMovimiento)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("tipo_movimiento");
            });

            modelBuilder.Entity<Operadore>(entity =>
            {
                entity.ToTable("operadores", "modulo");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Afore)
                    .HasMaxLength(280)
                    .IsUnicode(false)
                    .HasColumnName("afore")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ApellidoMaterno)
                    .IsRequired()
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("apellido_materno");

                entity.Property(e => e.ApellidoPaterno)
                    .IsRequired()
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("apellido_paterno");

                entity.Property(e => e.Contrasena)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("contrasena");

                entity.Property(e => e.CorreoElectronico)
                    .HasMaxLength(180)
                    .IsUnicode(false)
                    .HasColumnName("correo_electronico")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Curp)
                    .HasMaxLength(18)
                    .IsUnicode(false)
                    .HasColumnName("curp")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Direccion)
                    .HasMaxLength(380)
                    .IsUnicode(false)
                    .HasColumnName("direccion")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DispositivoInicioSesion)
                    .HasMaxLength(380)
                    .HasColumnName("dispositivo_inicio_sesion")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DispositivoNotificaciones)
                    .HasColumnType("text")
                    .HasColumnName("dispositivo_notificaciones")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.FechaBaja)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_baja");

                entity.Property(e => e.FechaInicioSesion)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_inicio_sesion");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_registro")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FkCausaBajaId).HasColumnName("fk_causa_baja_id");

                entity.Property(e => e.FkEstadoCivilId).HasColumnName("fk_estado_civil_id");

                entity.Property(e => e.FkEstadoOperadorId).HasColumnName("fk_estado_operador_id");

                entity.Property(e => e.FkExperienciaId).HasColumnName("fk_experiencia_id");

                entity.Property(e => e.FkNivelEcolaridadId).HasColumnName("fk_nivel_ecolaridad_id");

                entity.Property(e => e.FkPerfilId).HasColumnName("fk_perfil_id");

                entity.Property(e => e.FkPuestoId).HasColumnName("fk_puesto_id");

                entity.Property(e => e.FkSucursalId).HasColumnName("fk_sucursal_id");

                entity.Property(e => e.FkTipoContratoId).HasColumnName("fk_tipo_contrato_id");

                entity.Property(e => e.FkTipoId).HasColumnName("fk_tipo_id");

                entity.Property(e => e.FkUsuarioRegistro)
                    .HasColumnName("fk_usuario_registro")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.FotoPerfil)
                    .HasMaxLength(280)
                    .IsUnicode(false)
                    .HasColumnName("foto_perfil");

                entity.Property(e => e.Genero)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("genero")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength();

                entity.Property(e => e.KmsAcumulados)
                    .HasColumnName("kms_acumulados")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.NombreConyuge)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("nombre_conyuge")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Nss)
                    .HasColumnName("nss")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.NumeroEmpleado).HasColumnName("numero_empleado");

                entity.Property(e => e.Rfc)
                    .HasMaxLength(18)
                    .IsUnicode(false)
                    .HasColumnName("rfc")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.SisStatus)
                    .IsRequired()
                    .HasColumnName("sis_status")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.StatusSesion).HasColumnName("status_sesion");

                entity.Property(e => e.TelefonoCelular)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("telefono_celular")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.TelefonoEmergencias)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("telefono_emergencias")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.TotalHijos)
                    .HasColumnName("total_hijos")
                    .HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<OperadoresStatus>(entity =>
            {
                entity.ToTable("operadores_status", "modulo");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Descripcio)
                    .IsRequired()
                    .HasMaxLength(280)
                    .IsUnicode(false)
                    .HasColumnName("descripcio")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_registro")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FkUsuarioRegistro)
                    .HasColumnName("fk_usuario_registro")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.SisStatus)
                    .HasColumnName("sis_status")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("tipo");

                entity.Property(e => e.ValorExtra)
                    .IsRequired()
                    .HasMaxLength(280)
                    .IsUnicode(false)
                    .HasColumnName("valor_extra")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ValorExtra2)
                    .IsRequired()
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("valor_extra2")
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuarios", "sistema");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ApellidoMaterno)
                    .IsRequired()
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("apellido_materno");

                entity.Property(e => e.ApellidoPaterno)
                    .IsRequired()
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("apellido_paterno");

                entity.Property(e => e.CorreoElectronico)
                    .HasMaxLength(180)
                    .IsUnicode(false)
                    .HasColumnName("correo_electronico");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.SisStatus)
                    .IsRequired()
                    .HasColumnName("sis_status")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.StatusSesion).HasColumnName("status_sesion");
            });

            modelBuilder.Entity<AppOpAztecaAcceso>(entity =>
            {
                entity.ToTable("perfil_accesos", "seguridad");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FkPerfilId).HasColumnName("fk_perfil_id");
                entity.Property(e => e.FkModuloProcesoId).HasColumnName("fk_modulo_proceso_id");
                entity.Property(e => e.Tipo)
                   .IsRequired()
                   .HasColumnName("tipo")
                   .HasDefaultValueSql("((1))");
            });
            
            modelBuilder.Entity<AppOpAztecaViajes>(entity =>
            {
                entity.ToTable("controlViajes_viajes", "modulo");

                entity.Property(e => e.Id).HasColumnName("id");
                
                entity.Property(e => e.NumeroViaje)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("numero_viaje");
                entity.Property(e => e.NumeroLiquidacion).HasColumnName("numero_liquidacion");
                
                entity.Property(e => e.RutaNombre)
                    .HasMaxLength(180)
                    .IsUnicode(false)
                    .HasColumnName("ruta_nombre");
                
                entity.Property(e => e.ClienteRemitente)
                    .HasMaxLength(180)
                    .IsUnicode(false)
                    .HasColumnName("cliente_remitente");
                
                entity.Property(e => e.ClienteDestinario)
                    .HasMaxLength(180)
                    .IsUnicode(false)
                    .HasColumnName("cliente_destinario");

                entity.Property(e => e.PesoToneladas)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("peso_toneladas")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.TipoArmado)
                   .HasMaxLength(80)
                   .IsUnicode(false)
                   .HasColumnName("Tipo_armado");
                
                entity.Property(e => e.ClaveUnidad)
                   .HasMaxLength(80)
                   .IsUnicode(false)
                   .HasColumnName("clave_unidad");

                entity.Property(e => e.ClaveRemolque1)
                  .HasMaxLength(80)
                  .IsUnicode(false)
                  .HasColumnName("clave_remolque1");

                entity.Property(e => e.ClaveDolly)
                  .HasMaxLength(80)
                  .IsUnicode(false)
                  .HasColumnName("clave_dolly");

                entity.Property(e => e.ClaveRemolque2)
                  .HasMaxLength(80)
                  .IsUnicode(false)
                  .HasColumnName("clave_remolque2");

                entity.Property(e => e.KilometrajeInicial)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("kilometraje_inicial");

                entity.Property(e => e.KilometrajeFinal)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("kilometraje_final");

                entity.Property(e => e.KilometrosRutaManual)
                 .HasColumnType("decimal(18, 2)")
                 .HasColumnName("kilometros_ruta_manual");

                entity.Property(e => e.KilometrosRecorridos)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("kilometros_recorridos");

                entity.Property(e => e.FechaInicio)
                  .HasMaxLength(120)
                  .IsUnicode(false)
                  .HasColumnName("fecha_inicio");

                entity.Property(e => e.FechaFin)
                  .HasMaxLength(120)
                  .IsUnicode(false)
                  .HasColumnName("fecha_fin");

                entity.Property(e => e.FkEstadoDocumentosId).HasColumnName("fk_estado_documentos_id");
                entity.Property(e => e.EstadoDocumento)
                 .HasMaxLength(120)
                 .IsUnicode(false)
                 .HasColumnName("estado_documento");
                entity.Property(e => e.EstadoDocumentoColor)
                 .HasMaxLength(120)
                 .IsUnicode(false)
                 .HasColumnName("estado_documento_color");
              
                entity.Property(e => e.TotalDocumentos).HasColumnName("total_documentos");
                entity.Property(e => e.TotalDocumentosAprobados).HasColumnName("total_documentos_aprobados");

                entity.Property(e => e.FkEstadoViajeId).HasColumnName("fk_estado_viaje");
                entity.Property(e => e.EstadoViaje)
                 .HasMaxLength(120)
                 .IsUnicode(false)
                 .HasColumnName("estado_viaje");
            });

            modelBuilder.Entity<ViajeDocumentos>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DocumentoNombre)
                    .HasMaxLength(380)
                    .IsUnicode(false)
                    .HasColumnName("DocumentoNombre");

                entity.Property(e => e.UrlDocumento)
                    .HasMaxLength(380)
                    .IsUnicode(false)
                    .HasColumnName("url_documento_operador");

                entity.Property(e => e.ClienteNombre)
                   .HasMaxLength(380)
                   .IsUnicode(false)
                   .HasColumnName("ClienteNombre");

                entity.Property(e => e.FkEstadoId).HasColumnName("fk_estado_id");

                entity.Property(e => e.EstadoNombre)
                   .HasMaxLength(380)
                   .IsUnicode(false)
                   .HasColumnName("EstadoDocumento");

                entity.Property(e => e.EstadoColor)
                  .HasMaxLength(120)
                  .IsUnicode(false)
                  .HasColumnName("EstadoDocumentoColor");
            });


            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}