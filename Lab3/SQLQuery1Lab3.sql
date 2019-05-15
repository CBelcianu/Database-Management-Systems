USE ArtGallery

go

--a)
create or alter function uf_validateFname (@fname varchar(20)) returns int as
begin
declare @return int
set @return = 1
if UNICODE(@fname) = UNICODE(LOWER(@fname)) set @return = 0
return @return
end

go

create or alter function uf_validateLname (@lname varchar(20)) returns int as
begin
declare @return int
set @return = 1
if UNICODE(@lname) = UNICODE(LOWER(@lname)) set @return = 0
return @return
end

go

create or alter function uf_validateType (@type varchar(20)) returns int as
begin
declare @return int
set @return = 1
if UNICODE(@type) = UNICODE(LOWER(@type)) set @return = 0
return @return
end

go

create or alter proc AddSectionSupervisor @type varchar(30), @fname varchar(20), @lname varchar(20) as
begin
	begin tran
	begin try
		if(dbo.uf_validateType(@type) <> 1)
		begin 
			raiserror('Invalid Section Type!',14,1)
		end
		if(dbo.uf_validateFname(@fname) <> 1)
		begin 
			raiserror('Invalid First Name!',14,1)
		end
		if(dbo.uf_validateLname(@lname) <> 1)
		begin 
			raiserror('Invalid Last Name',14,1)
		end
		declare @supid int, @secid int
		select @supid = max(SupervisorID) from Supervisors
		select @secid = max(SectionID) from Sections
		insert into Sections(SectionID,SectionType) values (@secid+1, @type)
		insert into Supervisors(SupervisorID,SupervisorFname,SupervisorLname) values(@supid+1,@fname,@lname)
		commit tran
		select 'Transaction commited'
	end try
	begin catch
		rollback tran
		select 'Transaction rollbacked'
	end catch
end

go

declare @thedate datetime
set @thedate = GETDATE()
insert into LogTable(TypeOperation,ExecutionDate) values ('AddSectionSupervisor',@thedate)
exec AddSectionSupervisor 'notgud', 'notgud', 'notgud'
set @thedate = GETDATE()
insert into LogTable(TypeOperation,ExecutionDate) values ('AddSectionSupersivor',@thedate)
exec AddSectionSupervisor 'Gud','Gud','Gud'


select * from Supervisors
select * from Sections
select * from LogTable

delete from Supervisors where SupervisorFname='Gud'
delete from Sections where SectionType='Gud'

--b)
go

create or alter proc AddSectionSupervisorBetter @type varchar(30), @fname varchar(20), @lname varchar(20) as
begin
	begin tran
	begin try
		if(dbo.uf_validateType(@type) <> 1)
		begin 
			raiserror('Invalid Section Type!',14,1)
		end
		declare @secid int
		select @secid = max(SectionID) from Sections
		insert into Sections(SectionID,SectionType) values (@secid+1, @type)
		commit tran
		select 'Transaction commited'
	end try
	begin catch
		rollback tran
		select 'Transaction rollbacked'
	end catch

	begin tran
	begin try
		if(dbo.uf_validateFname(@fname) <> 1)
		begin 
			raiserror('Invalid First Name!',14,1)
		end
		if(dbo.uf_validateLname(@lname) <> 1)
		begin 
			raiserror('Invalid Last Name',14,1)
		end
		declare @supid int
		select @supid = max(SupervisorID) from Supervisors
		insert into Supervisors(SupervisorID,SupervisorFname,SupervisorLname) values(@supid+1,@fname,@lname)
		commit tran
		select 'Transaction commited'
	end try
	begin catch
		rollback tran
		select 'Transaction rollbacked'
	end catch

end

go

declare @thedate datetime
set @thedate = GETDATE()
insert into LogTable(TypeOperation,ExecutionDate) values ('AddSectionSupervisorBetter',@thedate)
exec AddSectionSupervisorBetter'Gud1', 'notgud', 'notgud'
set @thedate = GETDATE()
insert into LogTable(TypeOperation,ExecutionDate) values ('AddSectionSupervisorBetter',@thedate)
exec AddSectionSupervisorBetter 'Gud2','Gud2','Gud2'

select * from Supervisors
select * from Sections
select * from LogTable

delete from Supervisors where SupervisorID=4
delete from Sections where SectionID=8 or SectionID=9

update Supervisors set SupervisorFname = 'Testf' where SupervisorID=3
update Sections set SectionType = 'Before' where SectionID=4
update Supervisors set SupervisorFname = 'Before' where SupervisorID=4
delete from Supervisors where SupervisorID=5

go
--c)

--DIRTY READS

declare @thedate datetime
set @thedate = GETDATE()
insert into LogTable(TypeOperation,ExecutionDate) values ('DirtyReadT1',@thedate)
begin tran
update Supervisors set SupervisorFname = 'DirtyRead'
where SupervisorID = 2
waitfor delay '00:00:10'
rollback tran

go
--NON-REPEATABLE READS


declare @thedate datetime
set @thedate = GETDATE()
insert into LogTable(TypeOperation,ExecutionDate) values ('Non-RepeatableReadT1',@thedate)
INSERT INTO Supervisors(SupervisorID, SupervisorFname, SupervisorLname) VALUES (4,'Nonrep','Nonrep')
BEGIN TRAN
WAITFOR DELAY '00:00:05'
UPDATE Supervisors SET SupervisorFname='Nonrepread' WHERE SupervisorLname = 'Nonrep'
COMMIT TRAN

go
--PHANOTM READS

declare @thedate datetime
set @thedate = GETDATE()
insert into LogTable(TypeOperation,ExecutionDate) values ('PhantomReadT1',@thedate)
BEGIN TRAN
WAITFOR DELAY '00:00:04'
INSERT INTO Supervisors(SupervisorID, SupervisorFname, SupervisorLname) VALUES (5,'Phantom','Phantom')
COMMIT TRAN

go
--DEADLOCK

-- transaction 1
declare @thedate datetime
set @thedate = GETDATE()
insert into LogTable(TypeOperation,ExecutionDate) values ('DeadlockT1',@thedate)
begin tran
update Supervisors set SupervisorFname='FN transaction 1' where SupervisorID=3
waitfor delay '00:00:10'
update Sections set SectionType='Type transaction 1' where SectionID=4
commit tran


update Supervisors set SupervisorFname = 'Testf' where SupervisorID=3
update Sections set SectionType = 'Before' where SectionID=4

select * from Supervisors
select * from Sections
select * from LogTable