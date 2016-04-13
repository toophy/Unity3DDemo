// easybuff
// 不要修改本文件, 每次消息有变动, 请手动生成本文件
// easybuff -s 描述文件目录 -o 目标文件目录 -l 语言(go,cpp,c#)

using NetMsg;
using System;

namespace NetMsg
{
    // ------ 枚举
    // ------ 消息ID
    enum MsgId_chat_proto
    {
        C2G_login_Id = 1, // 登录聊天服务器
        G2C_login_ret_Id = 2, // 响应登录
        C2G_createRole_Id = 3, // 创建角色
        G2C_createRole_ret_Id = 4, // 响应创建角色
        C2S_chat_Id = 5, // 发送聊天信息
        C2C_chat_private_Id = 6, // 发送私聊信息
        S2C_chat_Id = 7, // 返回聊天信息
        S2C_chat_private_Id = 8, // 返回私人聊天信息
        S2G_more_packet_Id = 9, // Server发送给Gate消息包
        G2S_more_packet_Id = 10, // Gate发送给GServer消息包
        S2G_registe_Id = 12, // Server向Gate注册
        G2S_registe_Id = 13, // Gate向Server返回注册结果
        S2C_monsterData_Id = 14, // Server向Client返回怪物信息
    }
    // ------ 普通结构
    // 位置数据
    class TVec3
    {
        public float X; // x坐标
        public float Y; // y坐标
        public float Z; // z坐标
        public TVec3()
        {
        }
        public void Read(ref PacketReader p)
        {
            X = p.ReadFloat32();
            Y = p.ReadFloat32();
            Z = p.ReadFloat32();
        }
        public void Write(ref PacketWriter p)
        {
            p.WriteFloat32(X);
            p.WriteFloat32(Y);
            p.WriteFloat32(Z);
        }
    }
    // 怪物数据
    class MonsterData
    {
        public string Name; // 怪物名
        public TVec3 CurrPos; // 当前位置
        public TVec3 TargetPos; // 目标位置
        public float Speed; // 速度
        public MonsterData()
        {
            CurrPos = new TVec3();
            TargetPos = new TVec3();
        }
        public void Read(ref PacketReader p)
        {
            p.ReadString(ref Name);
            CurrPos.Read(ref p);
            TargetPos.Read(ref p);
            Speed = p.ReadFloat32();
        }
        public void Write(ref PacketWriter p)
        {
            p.WriteString(ref Name);
            CurrPos.Write(ref p);
            TargetPos.Write(ref p);
            p.WriteFloat32(Speed);
        }
    }

    // ------ 消息结构
    // 登录聊天服务器
    class C2G_login
    {
        public string Account; // 帐号
        public int Time; // 登录时间戳
        public string Sign; // 验证码

        public C2G_login()
        {
        }
        public void Read(ref PacketReader p)
        {
            p.ReadString(ref Account);
            Time = p.ReadInt32();
            p.ReadString(ref Sign);
        }
        public void Write(ref PacketWriter p)
        {
            p.WriteMsgId((ushort)MsgId_chat_proto.C2G_login_Id);
            p.WriteString(ref Account);
            p.WriteInt32(Time);
            p.WriteString(ref Sign);
            p.WriteMsgOver();
        }
    }
    // 响应登录
    class G2C_login_ret
    {
        public sbyte Ret; // 登录结果,0:成功,其他为失败原因
        public string Msg; // 登录失败描述

        public G2C_login_ret()
        {
        }
        public void Read(ref PacketReader p)
        {
            Ret = p.ReadInt8();
            p.ReadString(ref Msg);
        }
        public void Write(ref PacketWriter p)
        {
            p.WriteMsgId((ushort)MsgId_chat_proto.G2C_login_ret_Id);
            p.WriteInt8(Ret);
            p.WriteString(ref Msg);
            p.WriteMsgOver();
        }
    }
    // 创建角色
    class C2G_createRole
    {
        public string Name; // 角色名
        public sbyte Sex; // 性别

        public C2G_createRole()
        {
        }
        public void Read(ref PacketReader p)
        {
            p.ReadString(ref Name);
            Sex = p.ReadInt8();
        }
        public void Write(ref PacketWriter p)
        {
            p.WriteMsgId((ushort)MsgId_chat_proto.C2G_createRole_Id);
            p.WriteString(ref Name);
            p.WriteInt8(Sex);
            p.WriteMsgOver();
        }
    }
    // 响应创建角色
    class G2C_createRole_ret
    {
        public sbyte Ret; // 创建角色结果,0:成功,其他为失败原因
        public string Msg; // 创建角色失败描述

        public G2C_createRole_ret()
        {
        }
        public void Read(ref PacketReader p)
        {
            Ret = p.ReadInt8();
            p.ReadString(ref Msg);
        }
        public void Write(ref PacketWriter p)
        {
            p.WriteMsgId((ushort)MsgId_chat_proto.G2C_createRole_ret_Id);
            p.WriteInt8(Ret);
            p.WriteString(ref Msg);
            p.WriteMsgOver();
        }
    }
    // 发送聊天信息
    class C2S_chat
    {
        public int Channel; // 频道
        public string Data; // 聊天信息

        public C2S_chat()
        {
        }
        public void Read(ref PacketReader p)
        {
            Channel = p.ReadInt32();
            p.ReadString(ref Data);
        }
        public void Write(ref PacketWriter p)
        {
            p.WriteMsgId((ushort)MsgId_chat_proto.C2S_chat_Id);
            p.WriteInt32(Channel);
            p.WriteString(ref Data);
            p.WriteMsgOver();
        }
    }
    // 发送私聊信息
    class C2C_chat_private
    {
        public string Target; // 聊天目标
        public string Data; // 聊天信息

        public C2C_chat_private()
        {
        }
        public void Read(ref PacketReader p)
        {
            p.ReadString(ref Target);
            p.ReadString(ref Data);
        }
        public void Write(ref PacketWriter p)
        {
            p.WriteMsgId((ushort)MsgId_chat_proto.C2C_chat_private_Id);
            p.WriteString(ref Target);
            p.WriteString(ref Data);
            p.WriteMsgOver();
        }
    }
    // 返回聊天信息
    class S2C_chat
    {
        public int Channel; // 频道
        public string Source; // 发言人
        public string Data; // 聊天信息

        public S2C_chat()
        {
        }
        public void Read(ref PacketReader p)
        {
            Channel = p.ReadInt32();
            p.ReadString(ref Source);
            p.ReadString(ref Data);
        }
        public void Write(ref PacketWriter p, long tgid)
        {
            p.WriteMsgId((ushort)MsgId_chat_proto.S2C_chat_Id);
            p.WriteInt32(Channel);
            p.WriteString(ref Source);
            p.WriteString(ref Data);
            p.WriteMsgOver();
        }
    }
    // 返回私人聊天信息
    class S2C_chat_private
    {
        public string Source; // 发言人
        public string Target; // 倾听者
        public string Data; // 聊天信息

        public S2C_chat_private()
        {
        }
        public void Read(ref PacketReader p)
        {
            p.ReadString(ref Source);
            p.ReadString(ref Target);
            p.ReadString(ref Data);
        }
        public void Write(ref PacketWriter p, long tgid)
        {
            p.WriteMsgId((ushort)MsgId_chat_proto.S2C_chat_private_Id);
            p.WriteString(ref Source);
            p.WriteString(ref Target);
            p.WriteString(ref Data);
            p.WriteMsgOver();
        }
    }
    // Server发送给Gate消息包
    class S2G_more_packet
    {

        public S2G_more_packet()
        {
        }
        public void Read(ref PacketReader p)
        {
        }
        public void Write(ref PacketWriter p, long tgid)
        {
            p.WriteMsgId((ushort)MsgId_chat_proto.S2G_more_packet_Id);
            p.WriteMsgOver();
        }
    }
    // Gate发送给GServer消息包
    class G2S_more_packet
    {

        public G2S_more_packet()
        {
        }
        public void Read(ref PacketReader p)
        {
        }
        public void Write(ref PacketWriter p, long tgid)
        {
            p.WriteMsgId((ushort)MsgId_chat_proto.G2S_more_packet_Id);
            p.WriteMsgOver();
        }
    }
    // Server向Gate注册
    class S2G_registe
    {
        public ulong Sid; // 小区编号

        public S2G_registe()
        {
        }
        public void Read(ref PacketReader p)
        {
            Sid = p.ReadUint64();
        }
        public void Write(ref PacketWriter p, long tgid)
        {
            p.WriteMsgId((ushort)MsgId_chat_proto.S2G_registe_Id);
            p.WriteUint64(Sid);
            p.WriteMsgOver();
        }
    }
    // Gate向Server返回注册结果
    class G2S_registe
    {
        public byte Ret; // 返回结果,0:成功,其他失败
        public string Msg; // 返回失败原因

        public G2S_registe()
        {
        }
        public void Read(ref PacketReader p)
        {
            Ret = p.ReadUint8();
            p.ReadString(ref Msg);
        }
        public void Write(ref PacketWriter p, long tgid)
        {
            p.WriteMsgId((ushort)MsgId_chat_proto.G2S_registe_Id);
            p.WriteUint8(Ret);
            p.WriteString(ref Msg);
            p.WriteMsgOver();
        }
    }
    // Server向Client返回怪物信息
    class S2C_monsterData
    {
        public MonsterData Data; // 怪物数据

        public S2C_monsterData()
        {
            Data = new MonsterData();
        }
        public void Read(ref PacketReader p)
        {
            Data.Read(ref p);
        }
        public void Write(ref PacketWriter p, long tgid)
        {
            p.WriteMsgId((ushort)MsgId_chat_proto.S2C_monsterData_Id);
            Data.Write(ref p);
            p.WriteMsgOver();
        }
    }
}
