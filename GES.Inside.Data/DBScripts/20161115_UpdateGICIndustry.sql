


SET IDENTITY_INSERT I_Msci ON
if not exists (select * from I_Msci where I_Msci_Id = 269)
	insert into I_Msci (I_Msci_Id, ParentI_Msci_Id, Code, Name) values (269, 269, '60', 'Real Estate')
SET IDENTITY_INSERT I_Msci OFF

go

SET IDENTITY_INSERT I_Msci_Tree ON
if not exists (select * from I_Msci_Tree where I_Msci_Tree_Id = 269)
	insert into I_Msci_Tree (I_Msci_Tree_Id, ParentI_Msci_Tree_Id, I_Msci_Id, Depth, Lineage) values (269, NULL, 269, 0, '/91/0')
SET IDENTITY_INSERT I_Msci_Tree OFF

update I_Msci set ParentI_Msci_Id = 269, Code = 6010
where I_Msci_Id = 29

update I_Msci
set Code = REPLACE(Code, '4040', '6010')
where Code like	'4040%'

update I_Msci_Tree
set Lineage = '/91/170/'
where I_Msci_Id = 29

update I_Msci_Tree
set Lineage = REPLACE(Lineage, '/154/170/', '/91/170/')
where Lineage like '/154/170/%'
