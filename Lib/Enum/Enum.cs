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
        Background,
        ScanId,
        instruction,
        RoundOne,
        RoundTwo,
        RoundThree,
        RoundFour,
        RoundFive,
        MissionAccomplished,
        DoubleScore,
        LightsChange,
        Charge,
    }
    public enum Room
    {
        Fort,
        Dark,
        Diving,
        FloorIsLava,
        Gathering,
        Shooting,
    }
    public enum Displays
    {
        first = 1,
        second,
        third,
        fourth,
        fifth,
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
        White,
        Turquoise,
        Cyan,
        purple,
        Yellow,
        Magenta,
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
        Stop = 0,
        Motor1Round1 = 1000,
        Motor1Round2 = 1250,
        Motor1Round3 = 1500,
        Motor1Round4 = 1750,
        Motor1Round5 = 2000,

        Motor2Round1 = 1000,
        Motor2Round2 = 1250,
        Motor2Round3 = 1500,
        Motor2Round4 = 1750,
        Motor2Round5 = 2000,

        Motor3Round1 = 2000,
        Motor3Round2 = 2500,
        Motor3Round3 = 3000,
        Motor3Round4 = 3500,
        Motor3Round5 = 4000,

        Motor4Round1 = 1000,
        Motor4Round2 = 1000,
        Motor4Round3 = 1000,
        Motor4Round4 = 1000,
        Motor4Round5 = 1000,



        Slow = 1000,
        Medium = 1500,
        High = 2000,
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
    public enum Target
    {
        Target1 = 1,
        Target2 = 2,
        Target3 = 3,
        Target4 = 4,
        Target5 = 5,
    }

    public enum Round
    {
        Round0 = -1,
        Round1 = 0,
        Round2 = 1,
        Round3 = 2,
        Round4 = 3,
        Round5 = 4
    }
    public enum Level
    {
        Level1 = 0,
        Level2 = 1,
        Level3 = 2,
        Level4 = 3,
        Level5 = 4,
        Finished = 5,
    }

    public enum GameStatus
    {
        Empty,
        NotStarted, // Not Empty and Not Started 
        StartInstructionAudio,
        InstructionAudioStarted,
        InstructionAudioEnded,
        Started,
        FinishedNotEmpty, // Start Sending Request For the Next Room 
        ReadyToLeave, // Turn the PB Light
        Leaving,// PB Pressed waiting
        Reset,
    }

    public enum DoorStatus
    {
        Close,
        Open,
        Undefined
    }

}
