<script>
export default {
    props: {
        message: {
            type: String,
        },
    },
};
</script>

<template>
    <p>New {{ message }}</p>
</template>

<style lang="scss" scoped></style>
