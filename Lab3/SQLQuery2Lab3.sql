use ArtGallery

--DIRTY READS

--Unresolved

declare @thedate datetime
set @thedate = GETDATE()
insert into LogTable(TypeOperation,ExecutionDate) values ('DirtyReadT2U',@thedate)
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
BEGIN TRAN
SELECT * FROM Supervisors
WAITFOR DELAY '00:00:15'
SELECT * FROM Supervisors
COMMIT TRAN

go
--Solved

declare @thedate datetime
set @thedate = GETDATE()
insert into LogTable(TypeOperation,ExecutionDate) values ('DirtyReadT2R',@thedate)
SET TRANSACTION ISOLATION LEVEL READ COMMITTED
BEGIN TRAN
SELECT * FROM Supervisors
WAITFOR DELAY '00:00:15'
SELECT * FROM Supervisors
COMMIT TRAN

go
--NON-REPEATABLE READS

--Unresolved

declare @thedate datetime
set @thedate = GETDATE()
insert into LogTable(TypeOperation,ExecutionDate) values ('Non-RepeatableReadT2U',@thedate)
SET TRANSACTION ISOLATION LEVEL READ COMMITTED
BEGIN TRAN
SELECT * FROM Supervisors
WAITFOR DELAY '00:00:05'
SELECT * FROM Supervisors
COMMIT TRAN

go
--Solved

declare @thedate datetime
set @thedate = GETDATE()
insert into LogTable(TypeOperation,ExecutionDate) values ('Non-RepeatableReadT2R',@thedate)
SET TRANSACTION ISOLATION LEVEL REPEATABLE READ
BEGIN TRAN
SELECT * FROM Supervisors
WAITFOR DELAY '00:00:05'
SELECT * FROM Supervisors
COMMIT TRAN

go
--PHANTOM READS

--Unresolved

declare @thedate datetime
set @thedate = GETDATE()
insert into LogTable(TypeOperation,ExecutionDate) values ('PhantomReadT2U',@thedate)
SET TRANSACTION ISOLATION LEVEL REPEATABLE READ
BEGIN TRAN
SELECT * FROM Supervisors
WAITFOR DELAY '00:00:05'
SELECT * FROM Supervisors
COMMIT TRAN

go
--Solved

declare @thedate datetime
set @thedate = GETDATE()
insert into LogTable(TypeOperation,ExecutionDate) values ('PhantomReadT2R',@thedate)
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
BEGIN TRAN
SELECT * FROM Supervisors
WAITFOR DELAY '00:00:05'
SELECT * FROM Supervisors
COMMIT TRAN

go
--DEADLOCK

--Unsolved
--transaction2
declare @thedate datetime
set @thedate = GETDATE()
insert into LogTable(TypeOperation,ExecutionDate) values ('DeadlockT2U',@thedate)
begin tran
update Sections set SectionType='Type transaction 2' where SectionID=4
waitfor delay '00:00:10'
update Supervisors set SupervisorFname='FN transaction 1' where SupervisorID=3
commit tran

go
--Solved
--transaction2
declare @thedate datetime
set @thedate = GETDATE()
insert into LogTable(TypeOperation,ExecutionDate) values ('DeadlockT2R',@thedate)
set deadlock_priority high
begin tran
update Sections set SectionType='Type transaction 2' where SectionID=4
waitfor delay '00:00:10'
update Supervisors set SupervisorFname='FN transaction 1' where SupervisorID=3
commit tran