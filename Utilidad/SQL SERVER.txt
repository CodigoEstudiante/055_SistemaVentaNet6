

create database DBVENTA_CORE
go

USE DBVENTA_CORE

go


CREATE TABLE CATEGORIA(
IdCategoria int primary key identity,
Descripcion varchar(100),
FechaRegistro datetime default getdate()
)

go

CREATE TABLE PRODUCTO(
IdProducto int primary key identity,
Codigo varchar(100),
IdCategoria int,
Descripcion varchar(100),
PrecioCompra decimal(10,2),
PrecioVenta decimal(10,2),
Stock int,
FechaRegistro datetime default getdate()
)

go

create table VENTA(
IdVenta int primary key identity,
TipoPago varchar(50),
NumeroDocumento varchar(50),
DocumentoCliente varchar(50),
NombreCliente varchar(100),
MontoPagoCon decimal(10,2),
MontoCambio decimal(10,2),
MontoSubTotal  decimal(10,2),
MontoIGV  decimal(10,2),
MontoTotal decimal(10,2),
FechaRegistro datetime default getdate()
)


go


create table DETALLE_VENTA(
IdDetalleVenta int primary key identity,
IdVenta int,
IdProducto int,
PrecioVenta decimal(10,2),
Cantidad int,
Total decimal(10,2),
FechaRegistro datetime default getdate()
)

go

create table USUARIO(
IdUsuario int primary key identity,
NombreCompleto varchar(100),
Correo varchar(100),
Clave varchar(100),
FechaRegistro datetime default getdate()
)

go

insert into CATEGORIA(Descripcion) values
('Bebidas'),
('Embutidos'),
('Bazar'),
('Perifericos')

go

insert into PRODUCTO(Codigo,IdCategoria,Descripcion,PrecioCompra,PrecioVenta,Stock) values
('10101010',3,'Mermelada Gloria selecta 500mg','6.00','6.80',50),
('11111111',3,'Aceite primor 1.8lts','6.20','7.20',40),
('12121212',3,'Atun Real Light','4.00','5.20',30),
('13131313',3,'Refresco zuko Naranja','1.20','2.00',50),
('14141414',4,'Mouse logitech g230','120.00','130.00',20)


go

insert into USUARIO(NombreCompleto,Correo,Clave) values('codigo estudiante','codigo@gmail.com','123')

go

--=============================================================================================
-- PROCEDIMIENTOS ALMACENADOS -> PRODUCTO
--=============================================================================================
create procedure sp_listar_producto
as
begin
	select 
	p.IdProducto,
	p.Codigo,
	c.IdCategoria,c.Descripcion[DesCategoria],
	p.Descripcion,
	p.PrecioCompra,
	p.PrecioVenta,
	p.Stock
	from PRODUCTO p
	inner join CATEGORIA c on c.IdCategoria = p.IdCategoria
	order by p.IdProducto desc
end

go

create procedure sp_guardar_producto
(
@Codigo varchar(100),
@IdCategoria int,
@Descripcion varchar(100),
@PrecioCompra decimal(10,2),
@PrecioVenta decimal(10,2),
@Stock int
)
as
begin
	
	insert into PRODUCTO(Codigo,IdCategoria,Descripcion,PrecioCompra,PrecioVenta,Stock) values
	(@Codigo,@IdCategoria,@Descripcion,@PrecioCompra,@PrecioVenta,@Stock)

end

go

create procedure sp_editar_producto
(
@IdProducto int,
@Codigo varchar(100),
@IdCategoria int,
@Descripcion varchar(100),
@PrecioCompra decimal(10,2),
@PrecioVenta decimal(10,2),
@Stock int
)
as
begin
	update PRODUCTO set Codigo = @Codigo, IdCategoria = @IdCategoria, Descripcion = @Descripcion, PrecioCompra = @PrecioCompra,
	PrecioVenta = @PrecioVenta, Stock = @Stock where IdProducto = @IdProducto

end

go

create procedure sp_eliminar_producto
(
@IdProducto int
)
as
begin
	delete from PRODUCTO where IdProducto = @IdProducto
end

go

--=============================================================================================
-- PROCEDIMIENTOS ALMACENADOS -> CATEGORIA
--=============================================================================================

create procedure sp_listar_categoria
as
begin
	select IdCategoria,Descripcion from CATEGORIA

end

go

create procedure sp_guardar_categoria
(
@Descripcion varchar(100)
)
as
begin
	
	insert into CATEGORIA(Descripcion) values
	(@Descripcion)

end

go

create procedure sp_editar_categoria
(
@IdCategoria int,
@Descripcion varchar(100)
)
as
begin

	update CATEGORIA set Descripcion = @Descripcion where IdCategoria = @IdCategoria

end

go


create procedure sp_eliminar_categoria
(
@IdCategoria int
)
as
begin
	delete from CATEGORIA where IdCategoria = @IdCategoria
end

go

--=============================================================================================
-- PROCEDIMIENTOS ALMACENADOS -> USUARIO
--=============================================================================================


create procedure sp_listar_usuario
as
begin
	select IdUsuario,NombreCompleto,Correo,Clave from USUARIO

end

go

create procedure sp_guardar_usuario
(
@NombreCompleto varchar(100),
@Correo varchar(100),
@Clave varchar(100)
)
as
begin
	
	insert into USUARIO(NombreCompleto,Correo,Clave) values
	(@NombreCompleto,@Correo,@Clave)
end

go

create procedure sp_editar_usuario
(
@IdUsuario int,
@NombreCompleto varchar(100),
@Correo varchar(100),
@Clave varchar(100)
)
as
begin

	update USUARIO set NombreCompleto = @NombreCompleto, Correo = @Correo,Clave = @Clave
	where IdUsuario = @IdUsuario

end

go


create procedure sp_eliminar_usuario
(
@IdUsuario int
)
as
begin
	delete from USUARIO where IdUsuario = @IdUsuario
end

GO

--=============================================================================================
-- PROCEDIMIENTOS ALMACENADOS -> VENTA
--=============================================================================================

create procedure sp_registrar_venta(
@Venta_xml xml,
@NroDocumento varchar(6) output
)
as
begin

	
	declare @venta table (
	TipoPago varchar(50),
	NumeroDocumento varchar(50),
	DocumentoCliente varchar(50),
	NombreCliente varchar(50),
	MontoPagoCon decimal(10,2),
	MontoCambio decimal(10,2),
	MontoSubTotal decimal(10,2),
	MontoIGV decimal(10,2),
	MontoTotal decimal(10,2)
	)

	declare @detalleventa table (
	IdVenta int default 0,
	IdProducto int,
	PrecioVenta decimal(10,2),
	Cantidad int,
	Total decimal(10,2)
	)
	
	BEGIN TRY
		BEGIN TRANSACTION

		insert into @venta(TipoPago,NumeroDocumento,DocumentoCliente,NombreCliente,MontoPagoCon,MontoCambio,MontoSubTotal,MontoIGV,MontoTotal)
		select 
			nodo.elemento.value('TipoPago[1]','varchar(50)') as TipoPago,
			nodo.elemento.value('NumeroDocumento[1]','varchar(50)') as NumeroDocumento,
			nodo.elemento.value('DocumentoCliente[1]','varchar(50)') as DocumentoCliente,
			nodo.elemento.value('NombreCliente[1]','varchar(50)') as NombreCliente,
			nodo.elemento.value('MontoPagoCon[1]','decimal(10,2)') as MontoPagoCon,
			nodo.elemento.value('MontoCambio[1]','decimal(10,2)') as MontoCambio,
			nodo.elemento.value('MontoSubTotal[1]','decimal(10,2)') as MontoSubTotal,
			nodo.elemento.value('MontoIGV[1]','decimal(10,2)') as MontoIGV,
			nodo.elemento.value('MontoTotal[1]','decimal(10,2)') as MontoTotal
		from @Venta_xml.nodes('Venta') nodo(elemento)

		insert into @detalleventa(IdProducto,PrecioVenta,Cantidad,Total)
		select 
			nodo.elemento.value('IdProducto[1]','int') as IdProducto,
			nodo.elemento.value('PrecioVenta[1]','decimal(10,2)') as PrecioVenta,
			nodo.elemento.value('Cantidad[1]','int') as Cantidad,
			nodo.elemento.value('Total[1]','decimal(10,2)') as Total
		from @Venta_xml.nodes('Venta/Detalle_Venta/Item') nodo(elemento)


		--================================================
		-- EMPIEZA EL REGISTRO DE LA VENTA
		--================================================
		declare @identity as table(ID int)
		declare @id int = (SELECT isnull(max(IdVenta),0) +1 FROM VENTA)
		declare @tempnrodocumento varchar(50) = RIGHT('000000' + convert(varchar(max),@id),6)

		INSERT INTO VENTA(TipoPago,NumeroDocumento,DocumentoCliente,NombreCliente,MontoPagoCon,MontoCambio,MontoSubTotal,MontoIGV,MontoTotal)
		output inserted.IdVenta into @identity
		select TipoPago,@tempnrodocumento ,DocumentoCliente,NombreCliente,MontoPagoCon,MontoCambio,MontoSubTotal,MontoIGV,MontoTotal from @venta
		

		update @detalleventa set IdVenta = (select ID from @identity)

		insert into DETALLE_VENTA(IdVenta,IdProducto,PrecioVenta,Cantidad,Total)
		select IdVenta,IdProducto,PrecioVenta,Cantidad,Total from @detalleventa


		update p set p.Stock = p.Stock - dv.Cantidad from PRODUCTO p
		inner join @detalleventa dv on dv.IdProducto = p.IdProducto

		COMMIT
		set @NroDocumento = @tempnrodocumento

	END TRY
	BEGIN CATCH
		ROLLBACK
		set @NroDocumento = ''
	END CATCH

end

go


create proc sp_detalle_venta(
@nrodocumento varchar(50)
)
as
begin
		select
		v.TipoPago,
		v.NumeroDocumento,
		isnull(v.DocumentoCliente,'0')[DocumentoCliente],
		isnull(v.NombreCliente,'0')[NombreCliente],
		isnull(v.MontoPagoCon,'0')[MontoPagoCon],
		isnull(v.MontoCambio,'0')[MontoCambio],
		v.MontoSubTotal,
		v.MontoIGV,
		v.MontoTotal,
		convert(char(10),v.FechaRegistro,103)[FechaRegistro],
		(
			select p.Descripcion,dv.Cantidad,dv.PrecioVenta,dv.Total from DETALLE_VENTA dv
			inner join PRODUCTO p on dv.IdProducto = p.IdProducto
			where dv.IdVenta = v.IdVenta
			FOR XML PATH ('Item'),TYPE
		) [DetalleVenta]

		from venta v where v.NumeroDocumento= @nrodocumento
		FOR XML PATH ('') , ROOT('Venta') 
end

--=============================================================================================
-- PROCEDIMIENTOS ALMACENADOS -> REPORTE
--=============================================================================================

create proc sp_reporte_venta(
@fechaInicio varchar(10),
@fechaFin varchar(10)
)
as
begin

	set dateformat dmy
	
	select v.TipoPago,v.NumeroDocumento,v.MontoTotal,
	--concat(convert(char(10),v.FechaRegistro,103),' ',convert(char(8),v.FechaRegistro,108))[FechaRegistro],
	convert(char(10),v.FechaRegistro,103)[FechaRegistro],
	p.Descripcion[DesProducto], dv.Cantidad,dv.PrecioVenta,dv.Total
	from VENTA v
	inner join DETALLE_VENTA dv on v.IdVenta = dv.IdVenta
	inner join PRODUCTO p on p.IdProducto = dv.IdProducto
	where convert(date, v.FechaRegistro) between @fechaInicio and @fechaFin
end


