-- SQLite
-- borrar todos los registros
DELETE FROM Transactions;
DELETE FROM Accounts;
DELETE FROM Customers;

-- para reiniciar el contador AUTOINCREMENT
DELETE FROM sqlite_sequence WHERE name IN ('Transactions', 'Accounts', 'Customers');
DELETE FROM sqlite_sequence;
