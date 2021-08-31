using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using System.Linq;

namespace Bit.Client.Web.BlazorUI
{
    public partial class BitPivot
    {
        private string? selectedKey;
        private OverflowBehavior overflowBehavior = OverflowBehavior.None;
        private LinkFormat linkFormat = LinkFormat.Links;
        private LinkSize linkSize = LinkSize.Normal;
        private bool SelectedKeyHasBeenSet;
        private BitPivotItem? SelectedItem { get; set; }
        private List<BitPivotItem> AllItems = new(); 

        /// <summary>
        /// Default selected key for the pivot
        /// </summary>
        [Parameter]
        public string DefaultSelectedKey { get; set; } = "0";

        /// <summary>
        /// The content of pivot, It can be Any custom tag
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// Overflow behavior when there is not enough room to display all of the links/tabs
        /// </summary>
        [Parameter]
        public OverflowBehavior OverflowBehavior
        {
            get => overflowBehavior;
            set
            {
                overflowBehavior = value;
                ClassBuilder.Reset();
            }
        }

        /// <summary>
        /// Pivot link format, display mode for the pivot links
        /// </summary>
        [Parameter]
        public LinkFormat LinkFormat
        {
            get => linkFormat;
            set
            {
                linkFormat = value;
                ClassBuilder.Reset();
            }
        }

        /// <summary>
        /// Pivot link size
        /// </summary>
        [Parameter]
        public LinkSize LinkSize
        {
            get => linkSize;
            set 
            {
                linkSize = value;
                ClassBuilder.Reset();
            }
        }

        /// <summary>
        /// Whether to skip rendering the tabpanel with the content of the selected tab
        /// </summary>
        [Parameter]
        public bool HeadersOnly { get; set; } = false;

        /// <summary>
        /// Callback for when the selected pivot item is changed
        /// </summary>
        [Parameter]
        public EventCallback<BitPivotItem> OnLinkClick { get; set; }

        /// <summary>
        /// Key of the selected pivot item. Updating this will override the Pivot's selected item state
        /// </summary>
        [Parameter]
        public string SelectedKey
        {
            get => selectedKey;
            set
            {
                if (value == selectedKey) return;
                SelectItemByKey(value);
            }
        }

        /// <summary>
        /// Callback for when the selected key changed
        /// </summary>
        [Parameter]
        public EventCallback<string> SelectedKeyChanged { get; set; }

        internal async Task SelectItem(BitPivotItem item)
        {
            if (SelectedKeyHasBeenSet && SelectedKeyChanged.HasDelegate is false) return;

            SelectedItem?.SetState(false);
            item.SetState(true);

            SelectedItem = item;
            selectedKey = item.ItemKey;

            await OnLinkClick.InvokeAsync(item);
        }

        protected override Task OnInitializedAsync()
        {
            selectedKey = selectedKey ?? DefaultSelectedKey;
            return base.OnInitializedAsync(); 
        }

        protected override Task OnParametersSetAsync()
        {
            return base.OnParametersSetAsync();
        }

        protected override string RootElementClass => "bit-pvt";
        protected override void RegisterComponentClasses()
        {
            ClassBuilder.Register(() => LinkSize == LinkSize.Large ? $"{RootElementClass}-large-{VisualClassRegistrar()}"
                                      : LinkSize == LinkSize.Normal ? $"{RootElementClass}-normal-{VisualClassRegistrar()}"
                                      : string.Empty);

            ClassBuilder.Register(() => LinkFormat == LinkFormat.Links ? $"{RootElementClass}-links-{VisualClassRegistrar()}"
                                      : LinkFormat == LinkFormat.Tabs ? $"{RootElementClass}-tabs-{VisualClassRegistrar()}"
                                      : string.Empty);

            ClassBuilder.Register(() => OverflowBehavior == OverflowBehavior.Menu ? $"{RootElementClass}-menu-{VisualClassRegistrar()}"
                                      : OverflowBehavior == OverflowBehavior.Scroll ? $"{RootElementClass}-scroll-{VisualClassRegistrar()}"
                                      : OverflowBehavior == OverflowBehavior.None ? $"{RootElementClass}-none-{VisualClassRegistrar()}"
                                      : string.Empty);
        }

        internal void RegisterItem(BitPivotItem item)
        {
            if (IsEnabled is false)
            {
                item.IsEnabled = false;
            }

            if (SelectedKey == item.ItemKey)
            {
                item.SetState(true);
                SelectedItem = item;
            }
            AllItems.Add(item);
        }

        internal void UnregisterItem(BitPivotItem item)
        {
            AllItems.Remove(item);
        }

        private void SelectItemByKey(string? itemKey)
        {
            var newItem = AllItems.FirstOrDefault(i => i.ItemKey == itemKey);

            if (newItem == null || newItem == SelectedItem || newItem.IsEnabled is false)
            {
                _ = SelectedKeyChanged.InvokeAsync(selectedKey);
                return;
            }

            _ = SelectItem(newItem);
        }
    }
}
