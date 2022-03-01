<script setup>
import Badge from "primevue/badge";

const props = defineProps({
    items: {
        type: Array,
    },
    root: {
        type: Boolean,
        default: false,
    },
});

let activeIndex = $ref(0);
</script>

<script>
export default {
    name: "SidebarMenu",
};
</script>

<template>
    <ul v-if="props.items">
        <template v-for="(item, index) of props.items">
            <li
                :class="{
                    'layout-menuitem-category': root,
                    'active-menuitem': activeIndex === index && !item.to,
                }"
            >
                <template v-if="root">
                    <div class="layout-menuitem-root-text" :aria-label="item.label">
                        {{ item.label }}
                    </div>
                    <SidebarMenu :items="item.items" />
                </template>
                <template v-else>
                    <!-- Links to another route -->
                    <router-link
                        v-if="item.to"
                        :to="item.to"
                        :class="[item.class, 'p-ripple', { 'p-disabled': item.disabled }]"
                        :style="item.style"
                        :aria-label="item.label"
                        exact
                        role="menuitem"
                        v-ripple
                    >
                        <i :class="item.icon"></i>
                        <span>{{ item.label }}</span>
                        <i
                            v-if="item.items"
                            class="pi pi-fw pi-angle-down menuitem-toggle-icon"
                        ></i>
                        <Badge v-if="item.badge" :value="item.badge"></Badge>
                    </router-link>

                    <!-- Item for other commands -->
                    <a
                        v-if="!item.to"
                        :href="item.url || '#'"
                        :target="item.newTab ? '_blank' : ''"
                        :style="item.style"
                        :class="[item.class, 'p-ripple', { 'p-disabled': item.disabled }]"
                        :aria-label="item.label"
                        role="menuitem"
                        v-ripple
                    >
                        <i :class="item.icon"></i>
                        <span>{{ item.label }}</span>
                        <i
                            v-if="item.items"
                            class="pi pi-fw pi-angle-down menuitem-toggle-icon"
                        ></i>
                        <Badge v-if="item.badge" :value="item.badge"></Badge>
                    </a>
                </template>
            </li>
        </template>
    </ul>
</template>

<style lang="scss" scoped>
.layout-menuitem-root-text {
    font-size: 1.2rem !important;
}

.layout-menu li a {
    font-size: 1.1rem !important;

    &:focus {
        box-shadow: none;
    }
}
</style>
