using Library.Media;
using Library.RGBLib;
using Library;
using System.Device.Gpio;
using System.Diagnostics;
using Library.Modbus;
using Library.Enum;
using Library.PinMapping;

namespace FortRoom.Services
{
    public class ObstructionControlService : IHostedService, IDisposable
    {
        private readonly ILogger<ObstructionControlService> _logger;
        private CancellationTokenSource _cts1;

        public ObstructionControlService(ILogger<ObstructionControlService> logger)
        {
            _logger = logger;

        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start Obstruction Service");
            ObstructionLib.init(SerialPort.Serial);
            _cts1 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            Task task1 = Task.Run(() => RunService1(_cts1.Token));
            return Task.CompletedTask;
        }


        private async Task RunService1(CancellationToken cancellationToken)
        {
            while (true)
            {
                if (IsGameStartedOrInGoing())
                {
                    if (!VariableControlService.IsObstructionServiceStarted)
                        VariableControlService.IsObstructionServiceStarted = true;
                    try
                    {
                        if (!VariableControlService.IsThingsChangedForTheNewRound)
                        {
                            _logger.LogInformation("Change Motor Speed For the New Round");
                            ControlObstructionSpeed(VariableControlService.GameRound);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                    }


                }
                else if (!IsGameStartedOrInGoing() && VariableControlService.IsObstructionServiceStarted)
                {
                    StopObstructionService();
                }

                Thread.Sleep(10);
            }

        }


        private void ControlObstructionSpeed(Round round)
        {
            if (VariableControlService.TeamScore.isAdult)
            {
                switch (round)
                {
                    case Round.Round1:
                        if (IsGameStartedOrInGoing())
                            RunCommand(ModbusSlave.Slave1, MotorSpeed.Motor1Round1, MotorStatus.Run);
                        if (IsGameStartedOrInGoing())
                            RunCommand(ModbusSlave.Slave2, MotorSpeed.Motor2Round1, MotorStatus.Reverse);
                        if (IsGameStartedOrInGoing())
                            RunCommand(ModbusSlave.Slave3, MotorSpeed.Motor3Round1, MotorStatus.Reverse);
                        if (IsGameStartedOrInGoing())
                            RunCommand(ModbusSlave.Slave4, MotorSpeed.Motor4Round1, MotorStatus.Reverse);
                        break;
                    case Round.Round2:
                        if (IsGameStartedOrInGoing())
                            RunCommand(ModbusSlave.Slave1, MotorSpeed.Motor1Round2, MotorStatus.Run);
                        if (IsGameStartedOrInGoing())
                            RunCommand(ModbusSlave.Slave2, MotorSpeed.Motor2Round2, MotorStatus.Reverse);
                        if (IsGameStartedOrInGoing())
                            RunCommand(ModbusSlave.Slave3, MotorSpeed.Motor3Round2, MotorStatus.Reverse);
                        if (IsGameStartedOrInGoing())
                            RunCommand(ModbusSlave.Slave4, MotorSpeed.Motor4Round2, MotorStatus.Reverse);
                        break;
                    case Round.Round3:
                        if (IsGameStartedOrInGoing())
                            RunCommand(ModbusSlave.Slave1, MotorSpeed.Motor1Round3, MotorStatus.Run);
                        if (IsGameStartedOrInGoing())
                            RunCommand(ModbusSlave.Slave2, MotorSpeed.Motor2Round3, MotorStatus.Reverse);
                        if (IsGameStartedOrInGoing())
                            RunCommand(ModbusSlave.Slave3, MotorSpeed.Motor3Round3, MotorStatus.Reverse);
                        if (IsGameStartedOrInGoing())
                            RunCommand(ModbusSlave.Slave4, MotorSpeed.Motor4Round3, MotorStatus.Reverse);
                        break;
                    case Round.Round4:
                        if (IsGameStartedOrInGoing())
                            RunCommand(ModbusSlave.Slave1, MotorSpeed.Motor1Round4, MotorStatus.Run);
                        if (IsGameStartedOrInGoing())
                            RunCommand(ModbusSlave.Slave2, MotorSpeed.Motor2Round4, MotorStatus.Reverse);
                        if (IsGameStartedOrInGoing())
                            RunCommand(ModbusSlave.Slave3, MotorSpeed.Motor3Round4, MotorStatus.Reverse);
                        if (IsGameStartedOrInGoing())
                            RunCommand(ModbusSlave.Slave4, MotorSpeed.Motor4Round4, MotorStatus.Reverse);
                        break;
                }

            }
            else
            {
                if (IsGameStartedOrInGoing())
                    RunCommand(ModbusSlave.Slave1, MotorSpeed.Motor1Round1, MotorStatus.Run);
                if (IsGameStartedOrInGoing())
                    RunCommand(ModbusSlave.Slave2, MotorSpeed.Motor2Round1, MotorStatus.Reverse);
                if (IsGameStartedOrInGoing())
                    RunCommand(ModbusSlave.Slave3, MotorSpeed.Motor3Round1, MotorStatus.Reverse);
                if (IsGameStartedOrInGoing())
                    RunCommand(ModbusSlave.Slave4, MotorSpeed.Motor4Round1, MotorStatus.Reverse);
            }


            VariableControlService.IsThingsChangedForTheNewRound = true;

        }
        private void RunCommand(ModbusSlave slave, MotorSpeed speed, MotorStatus status)
        {
            ObstructionLib.RunCommand(slave, speed, status);
            Thread.Sleep(500);
        }


        private bool IsGameStartedOrInGoing()
        {
            return VariableControlService.GameStatus == GameStatus.Started;
        }
        void StopObstructionService()
        {
            _logger.LogInformation("Obstruction Stopped ***");
            VariableControlService.IsObstructionServiceStarted = false;
            RunCommand(ModbusSlave.Slave1, MotorSpeed.Stop, MotorStatus.Stop);
            RunCommand(ModbusSlave.Slave2, MotorSpeed.Stop, MotorStatus.Stop);
            RunCommand(ModbusSlave.Slave3, MotorSpeed.Stop, MotorStatus.Stop);
            RunCommand(ModbusSlave.Slave4, MotorSpeed.Stop, MotorStatus.Stop);
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            Stopped();
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _cts1?.Dispose();
        }
        public void Stopped()
        {
            _logger.LogInformation("Obstruction Stopped");
            StopObstructionService();
            RGBLight.SetColor(VariableControlService.DefaultColor);
            ObstructionLib.Release();
            _logger.LogInformation("Obstruction - Port Released");
        }



    }
}
