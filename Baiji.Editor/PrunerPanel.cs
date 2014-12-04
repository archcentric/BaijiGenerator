﻿using CTripOSS.Baiji.IDLParser.Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace CTripOSS.Baiji.Editor
{
    public partial class PrunerPanel : UserControl, ISupportInitialize
    {
        private Service _service;

        public PrunerPanel()
        {
            InitializeComponent();
        }

        public Service Service
        {
            get
            {
                return _service;
            }
            set
            {
                _service = value;
                if (_service != null)
                {
                    foreach (var m in _service.Methods)
                    {
                        int index = m_OperationsCheckedListBox.Items.Add(m.Name);
                        m_OperationsCheckedListBox.SetItemChecked(index, true);
                    }
                }
            }
        }

        public IList<BaijiMethod> SelectedMethods
        {
            get
            {
                var selectedMethods = new List<BaijiMethod>();
                for (int i = 0; i < m_OperationsCheckedListBox.CheckedIndices.Count; i++)
                {
                    selectedMethods.Add(_service.Methods[i]);
                }
                return selectedMethods;
            }
        }

        public void ForceSelectAll()
        {
            for (int i = 0; i < m_OperationsCheckedListBox.Items.Count; i++)
            {
                m_OperationsCheckedListBox.SetItemChecked(i, true);
            }
            m_OperationsCheckedListBox.Enabled = false;
        }

        public void EnableSelect()
        {
            m_OperationsCheckedListBox.Enabled = true;
        }

        public void BeginInit()
        {
        }

        public void EndInit()
        {
        }
    }
}