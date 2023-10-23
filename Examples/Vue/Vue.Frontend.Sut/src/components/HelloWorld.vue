<template>
  <div class="post">
    <div v-if="loading" class="loading">
      Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationvue">https://aka.ms/jspsintegrationvue</a> for more details.
    </div>

    <div v-if="blogs" class="content">
      <table>
        <thead>
        <tr>
          <th>Url</th>
        </tr>
        </thead>
        <tbody>
        <tr v-for="blog in blogs" :key="blog.url">
          <td>
            <a href="{{ blog.url }}">{{ blog.url }}</a>
          </td>
        </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script lang="ts">
import {defineComponent} from 'vue';

type Forecasts = {
  url: string
}[];

interface Data {
  loading: boolean,
  blogs: null | Forecasts
}

export default defineComponent({
  data(): Data {
    return {
      loading: false,
      blogs: null
    };
  },
  created() {
    // fetch the data when the view is created and the data is
    // already being observed
    this.fetchData();
  },
  watch: {
    // call again the method if the route changes
    '$route': 'fetchData'
  },
  methods: {
    fetchData(): void {
      this.blogs = null;
      this.loading = true;

      fetch('blogs')
          .then(r => r.json())
          .then(json => {
            this.blogs = json as Forecasts;
            this.loading = false;
            return;
          });
    }
  },
});
</script>
