using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;


namespace YumDMBWindowsService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        private string _thumbPrint;

        private string _subscriptionId;
        private string _cloudserviceName;
        private string _deploymentsName;
        private string _vmNames;

        private int _timerInterval;
        private int _maxHitCount;

        //CPU利用率数组
        private float[] _cpuCounters;

        private float _maxCPU;
        private float _minCPU;

        private System.Timers.Timer _timer;
        private int _operationInterval;

        private int _index;
        private float totalCPUCounter;
        private bool _startComputeCPU;

        protected override void OnStart(string[] args)
        {
            if (InitAppConfig() && IniCertificate(_thumbPrint))
            {
                _timer.Interval = _timerInterval * 1000;
                _timer.Elapsed += _timer_Elapsed;
                _timer.Enabled = true;
            }
        }

        protected override void OnStop()
        {

        }

         private bool InitAppConfig()
        {
            try
            {
                _thumbPrint = ConfigurationManager.AppSettings["thumbprint"].ToString();

                _subscriptionId = ConfigurationManager.AppSettings["subscriptionid"].ToString();
                _cloudserviceName = ConfigurationManager.AppSettings["cloudservicename"].ToString();
                _deploymentsName = ConfigurationManager.AppSettings["deploymentsname"].ToString();
                _vmNames = ConfigurationManager.AppSettings["vmnames"].ToString();

                _timerInterval = Int32.Parse(ConfigurationManager.AppSettings["timerinterval"].ToString());
                _maxHitCount = Int32.Parse(ConfigurationManager.AppSettings["maxhitcount"].ToString());

                //初始化CPU利用率数组
                _cpuCounters = new float[_maxHitCount];

                _maxCPU = float.Parse(ConfigurationManager.AppSettings["maxcpu"].ToString());
                _minCPU = float.Parse(ConfigurationManager.AppSettings["mincpu"].ToString());

                if (_timer == null) _timer = new System.Timers.Timer();
              
                _operationInterval = Int32.Parse(ConfigurationManager.AppSettings["operationinterval"].ToString());

                _index = 0;
                totalCPUCounter = 0;
                _startComputeCPU = false;

                return true;
            }
            catch  (Exception ex)
            {
                Log.WriteLog("请检查App.config文件," + ex.Message);
                return false;
            }
        }

        private bool IniCertificate(string _certificateThumbprint)
        {
            return AzureVMOperation.CheckCertificate(_certificateThumbprint);
        }


        private float GetCPUCounter()
        {
            PerformanceCounter cpuCounter = new PerformanceCounter();
            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";

            // will always start at 0
            float firstValue = cpuCounter.NextValue();
            System.Threading.Thread.Sleep(1000);

            // now matches task manager reading
            float secondValue = cpuCounter.NextValue();

            return secondValue;
        }

        
        void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            float currentCPUCounter = GetCPUCounter();

            try
            {
                if (_index == 0 && currentCPUCounter >= _maxCPU)
                {
                    _startComputeCPU = true;
                }
                else if (_index == 0 && currentCPUCounter < _minCPU)
                {
                    _startComputeCPU = true;
                }

                if (_startComputeCPU)
                {
                    if (_index < _maxHitCount)
                    {
                        //取当前CPU
                        _cpuCounters[_index] = currentCPUCounter;
                        totalCPUCounter += currentCPUCounter;
                        _index += 1;
                    }
                    else
                    {
                        //计算平均利用率
                        float averageCPUCounter = totalCPUCounter / _maxHitCount;

                        //CPU利用率过高，且虚拟机都关机
                        //执行开机操作
                        _timer.Enabled = false;

                        if (averageCPUCounter >= _maxCPU)
                        {
                            string[] vms = _vmNames.Split(';');
                            foreach (string vm in vms)
                            {
                                //Start VM
                                AzureVMOperation.StartVM(_subscriptionId, _cloudserviceName, _deploymentsName, vm, _operationInterval * 1000);
                            }

                        }

                        //CPU利用率过低，且虚拟机都已经开机
                        else if (averageCPUCounter < _minCPU)
                        {
                            string[] vms = _vmNames.Split(';');
                            foreach (string vm in vms)
                            {
                                //Stop VM
                                AzureVMOperation.StopVM(_subscriptionId, _cloudserviceName, _deploymentsName, vm, true, _operationInterval * 1000);
                            }

                        }
                        _timer.Enabled = true;
                        _index = 0;
                        totalCPUCounter = 0;

                        _startComputeCPU = false;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
       
    }
}
