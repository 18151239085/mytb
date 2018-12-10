CREATE TABLE `tzb  帖子表` (
`id` varchar(25) NOT NULL COMMENT '主键',
`bt` varchar(255) NULL COMMENT '标题',
`content` varchar(255) NULL COMMENT '内容',
`create_time` datetime NULL COMMENT '创建时间',
`cjr` varchar(255) NULL COMMENT '创建人',
PRIMARY KEY (`id`) 
);

CREATE TABLE `人员表` (
`id` varchar(25) NOT NULL COMMENT '主键',
`nc` varchar(255) NULL COMMENT '昵称',
`yhm` varchar(255) NULL COMMENT '用户名',
`rylx` int(2) NULL COMMENT '人员类型(0是后台,1是前端用户)',
`mm` varchar(255) NULL COMMENT '密码',
`create_time` datetime NULL COMMENT '创建时间',
`sjh` varchar(255) NULL COMMENT '手机号',
PRIMARY KEY (`id`) 
);

CREATE TABLE `yjfk   意见反馈表` (
`id` varchar(25) NOT NULL,
`tzb_id` varchar(25) NULL COMMENT '帖子表id',
`hf_nr` varchar(255) NULL COMMENT '回复内容',
`hf_time` datetime NULL COMMENT '回复时间',
`ry_id` varchar(255) NULL COMMENT '人员id',
PRIMARY KEY (`id`) 
);

CREATE TABLE `附件表` (
`id` varchar(25) NOT NULL COMMENT '主键',
`tp_name` varchar(255) NULL COMMENT '图片名',
`tp_lj` varchar(255) NULL COMMENT '图片路径',
`ywlx` int(2) NULL COMMENT '业务类型(0是人员,1是帖子)',
`yw_id` varchar(25) NULL COMMENT '业务id',
`create_time` datetime NULL COMMENT '创建时间',
PRIMARY KEY (`id`) 
);

