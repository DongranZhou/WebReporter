import{o as l,c as _,a as m,F as E,r as I,t as V,b as S,n as N,J,d as i,e as c,w as d,f as y,p as O,g as b,h,i as f,j as K,k as T}from"./vendor.b1e2a6b8.js";const j=function(){const n=document.createElement("link").relList;if(n&&n.supports&&n.supports("modulepreload"))return;for(const t of document.querySelectorAll('link[rel="modulepreload"]'))r(t);new MutationObserver(t=>{for(const o of t)if(o.type==="childList")for(const s of o.addedNodes)s.tagName==="LINK"&&s.rel==="modulepreload"&&r(s)}).observe(document,{childList:!0,subtree:!0});function a(t){const o={};return t.integrity&&(o.integrity=t.integrity),t.referrerpolicy&&(o.referrerPolicy=t.referrerpolicy),t.crossorigin==="use-credentials"?o.credentials="include":t.crossorigin==="anonymous"?o.credentials="omit":o.credentials="same-origin",o}function r(t){if(t.ep)return;t.ep=!0;const o=a(t);fetch(t.href,o)}};j();var g=(e,n)=>{const a=e.__vccOpts||e;for(const[r,t]of n)a[r]=t;return a};const A={data(){return{logs:[],autoScroll:!0,selected:-1}},async created(){this.autoScroll=!0,this.logs=await this.$api.getLogs(),this.delayRefresh()},methods:{async delayRefresh(){await this.delay(.1),this.autoScroll&&(this.$refs.logview.scrollTop=this.$refs.logview.scrollHeight)},async delay(e){return new Promise(n=>{setTimeout(n,e*1e3)})}}},B=["onClick"],M={key:0,class:"info"};function P(e,n,a,r,t,o){return l(),_("div",null,[m("div",{class:"scroll",ref:"logview",onMouseenter:n[0]||(n[0]=s=>t.autoScroll=!1),onMouseleave:n[1]||(n[1]=s=>t.autoScroll=!0)},[(l(!0),_(E,null,I(t.logs,(s,u)=>(l(),_("div",{class:N(["item",{activeItem:u==t.selected,logItem:s.type=="Log",warningItem:s.type=="Warning",errorItem:s.type=="Error"}]),key:u,onClick:v=>t.selected=u},V(s.condition),11,B))),128))],544),t.selected>-1?(l(),_("pre",M,V(t.logs[t.selected].stackTrace),1)):S("",!0)])}var q=g(A,[["render",P],["__scopeId","data-v-74464fde"]]);const F={props:{name:String,modelValue:{type:[String,Number,Object,Array],default:()=>({})},modeType:{type:String,default:()=>"code"},modeList:{type:Array,default:()=>["code","view","tree"]}},data(){return{jsonEditor:null,internalChange:!1}},mounted(){this.init()},watch:{modelValue:{handler(e){this.internalChange||this.setValue(e)}}},methods:{init(){let e=this;this.jsonEditor=new J(e.$refs.view,{mode:this.modeType,modes:this.modeList,indentation:4,name:this.name,mainMenuBar:!0,onModeChange(){},onChange(){e.internalChange=!0,e.$emit("update:modelValue",e.jsonEditor.getText()),e.$nextTick(function(){e.internalChange=!1})}},e.modelValue)},setValue(e){this.jsonEditor&&(typeof e=="string"?this.jsonEditor.set(e?JSON.parse(e):""):this.jsonEditor.set(e|{}))}}},U={ref:"view"};function z(e,n,a,r,t,o){return l(),_("div",U,null,512)}var C=g(F,[["render",z]]);const R={components:{VueJsonEditor:C},data(){return{content:"",configKey:""}},mounted(){},methods:{async saveConfig(){var e=await this.$api.saveConfig(this.configKey,JSON.parse(this.content));console.log(e)},async querySearch(e,n){var a=await this.$api.getConfigs(),r=a.filter(o=>o.toLowerCase().indexOf(e.toLowerCase())>=0),t=r.map(o=>({value:o}));n(t)},async handleSelect(e){var n=await this.$api.getConfig({config:e.value});this.content=JSON.stringify(n)}}},D={class:"jsonmenu"},H=y("\u4FDD\u5B58");function W(e,n,a,r,t,o){const s=i("el-autocomplete"),u=i("el-button"),v=i("VueJsonEditor");return l(),_("div",null,[m("div",D,[c(s,{modelValue:t.configKey,"onUpdate:modelValue":n[0]||(n[0]=p=>t.configKey=p),"fetch-suggestions":o.querySearch,placeholder:"\u914D\u7F6E",onSelect:o.handleSelect},null,8,["modelValue","fetch-suggestions","onSelect"]),c(u,{onClick:o.saveConfig},{default:d(()=>[H]),_:1},8,["onClick"])]),m("div",null,[c(v,{modelValue:t.content,"onUpdate:modelValue":n[1]||(n[1]=p=>t.content=p)},null,8,["modelValue"])])])}var G=g(R,[["render",W],["__scopeId","data-v-71ed1918"]]);const Q={components:{VueJsonEditor:C},data(){return{content:""}},mounted(){this.getScene()},methods:{async getScene(){var e=await this.$api.getScene();this.content=JSON.stringify(e)}}},X=e=>(O("data-v-3db7c378"),e=e(),b(),e),Y=X(()=>m("div",{class:"jsonmenu"},null,-1));function Z(e,n,a,r,t,o){const s=i("VueJsonEditor");return l(),_("div",null,[Y,m("div",null,[c(s,{modeType:"view",modeList:["view"],modelValue:t.content,"onUpdate:modelValue":n[0]||(n[0]=u=>t.content=u)},null,8,["modelValue"])])])}var ee=g(Q,[["render",Z],["__scopeId","data-v-3db7c378"]]);const te={data(){return{activeIndex:"1",activeKey:"1"}},components:{LogView:q,ConfigView:G,SceneView:ee},methods:{handleSelect(e,n){this.activeKey=e}}},ne=y("\u65E5\u5FD7"),oe=y("\u914D\u7F6E"),se=y("\u573A\u666F");function re(e,n,a,r,t,o){const s=i("el-menu-item"),u=i("el-menu"),v=i("el-header"),p=i("LogView"),$=i("ConfigView"),x=i("SceneView"),L=i("el-main"),k=i("el-container");return l(),h(k,null,{default:d(()=>[c(v,null,{default:d(()=>[c(u,{"default-active":t.activeIndex,mode:"horizontal",onSelect:o.handleSelect},{default:d(()=>[c(s,{index:"1"},{default:d(()=>[ne]),_:1}),c(s,{index:"2"},{default:d(()=>[oe]),_:1}),c(s,{index:"3"},{default:d(()=>[se]),_:1})]),_:1},8,["default-active","onSelect"])]),_:1}),c(L,null,{default:d(()=>[t.activeKey=="1"?(l(),h(p,{key:0})):t.activeKey=="2"?(l(),h($,{key:1})):t.activeKey=="3"?(l(),h(x,{key:2})):S("",!0)]),_:1})]),_:1})}var ae=g(te,[["render",re]]),ie={async getLogs(e){var n=await f.get("/log",{params:e});return n.status==200?n.data:{}},async getConfigs(){var e=await f.get("/config");return e.status==200?e.data:{}},async getConfig(e){var n=await f.get("/config",{params:e});return n.status==200?n.data:{}},async saveConfig(e,n){var a={[e]:n},r=await f.post("/config",a);return r.status==200?r.data:{}},async getScene(){var e=await f.get("/scene");return e.status==200?e.data:{}}};const w=K(ae);w.config.globalProperties.$api=ie;w.use(T);w.mount("#app");
