create database ToDoListDB;

use ToDoListDB;

CREATE TABLE categories (
  id INT PRIMARY KEY AUTO_INCREMENT,
  name VARCHAR(255) NOT NULL
);

CREATE TABLE tasks (
  id INT PRIMARY KEY AUTO_INCREMENT,
  name VARCHAR(255) NOT NULL,
  due_date DATE,
  status BOOLEAN NOT NULL DEFAULT false,
  category_id INT NOT NULL,
  FOREIGN KEY (category_id) REFERENCES categories (id) ON DELETE CASCADE
);

insert into Categories values ('Study');
insert into Categories values ('Job');
insert into Categories values ('Family');
insert into Categories values ('Other');