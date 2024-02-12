using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public enum SoundType
    {
        Ticking = 1,
        Button,
        Done,
        Bonus,
        Finish,
        Start,
        Start2,
        LaserScan,
        LevelWin,
        MarioJump,
        MissionCompleted,
        Descend2,
        Descend,
        GameOver,
        GameScoreTally,
        WinSquare,
        Winner2,
        Winner1,
        Background

    }
    public enum Displays
    {
        first = 1,
        second,
        third,
        fourth
    }
    public enum DisplayCommand
    {
        clear = 0,
        rightArrow
    }
    public enum MCP23017
    {
        MCP2301720,
        MCP2301721,
        MCP2301722,
        MCP2301723,
        MCP2301724,
        MCP2301725,
        MCP2301726,
        MCP2301727,
    }
    public enum RGBColor
    {
        Red,
        Green,
        Blue,
        Off
    }
    public enum MotorStatus
    {
        Stop = 0,
        Run = 2,
        Reverse = 4,
    }
    public enum MotorSpeed
    {
        Slow = 200,
        Medium = 400,
        High = 600,
    }
    public enum ModbusAddress
    {
        Speed = 4,
        startStop = 5
    }
    public enum ModbusSlave
    {
        Slave1 = 1,
        Slave2 = 2,
        Slave3 = 3,
        Slave4 = 4,
    }

}
