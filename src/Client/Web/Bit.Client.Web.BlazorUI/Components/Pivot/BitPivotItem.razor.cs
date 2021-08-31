using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Bit.Client.Web.BlazorUI
{
    public partial class BitPivotItem
    {
        private bool IsSelectedHasBeenSet;

        [CascadingParameter(Name = "Pivot")] protected BitPivot? ParentPivot { get; set; }

        /// <summary>
        /// The content of the pivot item header, It can be Any custom tag or a text
        /// </summary>
        [Parameter]
        public RenderFragment HeaderContent { get; set; }

        /// <summary>
        /// The content of the pivot item, It can be Any custom tag or a text 
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// The content of the pivot item can be Any custom tag or a text, If HeaderContent provided value of this parameter show, otherwise use ChildContent
        /// </summary>
        [Parameter]
        public RenderFragment BodyContent { get; set; }

        /// <summary>
        /// The text of the pivot item header, The text displayed of each pivot link
        /// </summary>
        [Parameter]
        public string HeaderText { get; set; }

        /// <summary>
        /// The icon name for the icon shown next to the pivot link
        /// </summary>
        [Parameter]
        public string IconName { get; set; }

        /// <summary>
        /// Defines an optional item count displayed in parentheses just after the linkText
        /// </summary>
        [Parameter]
        public int? ItemCount { get; set; }

        /// <summary>
        /// A required key to uniquely identify a pivot item
        /// </summary>
        [Parameter]
        public string ItemKey { get; set; }

        [Parameter] public EventCallback<bool> IsSelectedChanged { get; set; }

        private bool isSelected;
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (value == isSelected) return;
                isSelected = value;
                ClassBuilder.Reset();
                _=IsSelectedChanged.InvokeAsync(value);
            }
        }

        internal void SetState(bool status)
        {
            IsSelected = status;
            StateHasChanged();
        }

        protected override Task OnInitializedAsync()
        {
            if (ParentPivot is not null)
            {
                ParentPivot.RegisterItem(this);
            }
            return base.OnInitializedAsync();
        }

        protected override string RootElementClass => "bit-pvt-itm";
        protected override void RegisterComponentClasses()
        {
            ClassBuilder.Register(() => IsSelected ? $"{RootElementClass}-selcted-{VisualClassRegistrar()}" : string.Empty);
        }

        private void HandleButtonClick()
        {
            if (IsEnabled is false) return;
            ParentPivot?.SelectItem(this);
        }
    }
}
