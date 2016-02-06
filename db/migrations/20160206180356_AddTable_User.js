{
	"up":{
		"create_table":{
			"name":"User","full_audit":false,
			"columns":[{
			"name":"Name","type":"string", "nullable": false},{
			"name":"Email","type":"string", "nullable": false},{
			"name":"Username","type":"string", "nullable": false},{
			"name":"IsAdmin","type":"bit", "nullable": false},{
			"name":"CreatedBy","type":"string", "nullable": false},{
			"name":"CreatedDate","type":"datetime", "nullable": false}]
		}
	},
	"down":{
		"drop_table":"User"}}