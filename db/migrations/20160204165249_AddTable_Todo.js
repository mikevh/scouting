{
	"up":{
		"create_table":{
			"name":"Todo","full_audit":false,
			"columns":[{
			"name":"Name","type":"string"},{
			"name":"IsDone","type":"bit", "nullable": false},{
			"name":"CreatedDate","type":"datetime", "nullable": false},{
			"name":"CreatedBy","type":"nvarchar(50)", "nullable":false}]
		}
	},
	"down":{
		"drop_table":"Todo"}}