create table articulos
(
    id_articulo int auto_increment primary key,
    nombre varchar(100) not null,
    descripcion varchar(200) not null,
    precio float(10,2) not null,
    cantidad int not null,
    foto longblob
);

create table carrito_compra
(
    id_articulo int auto_increment primary key,
    cantidad int not null
);

alter table carrito_compra add foreign key (id_articulo) references articulos(id_articulo);