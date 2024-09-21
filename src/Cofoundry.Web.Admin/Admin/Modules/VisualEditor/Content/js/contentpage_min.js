var Cofoundry=Cofoundry||{};
Cofoundry.events={on:function(s,t){this._callbacks||(this._callbacks={});(this._callbacks[s]||(this._callbacks[s]=[])).push(t)},off:function(s,t){for(var l=0,c=this._callbacks[s].length;l<c;l++)this._callbacks[s][l]===t&&this._callbacks[s].splice(l,1)},trigger:function(){var s,t,l,c=Array.prototype.slice.call(arguments,0),a=c.shift();if(!this._callbacks)return this;if(!(s=this._callbacks[a]))return this;for(t=0,l=s.length;t<l;t++)s[t].apply(this,c);return this}};
Cofoundry.visualEditor=function(){var L,i,s,M,D={bindIframe:function(){var e=document.getElementById("cofoundry-src__iFrame");document.getElementsByTagName("body")[0];(L=e).contentWindow.postMessage({action:"config",args:[!1,"EntityNameSingular"]},document.location.origin);e=window.addEventListener?"addEventListener":"attachEvent";(0,window[e])("attachEvent"==e?"onmessage":"message",function(e){"MODAL_CLOSE"===e.data.type&&(L.style.display="none")})},bindToolbar:function(){var e=document.getElementById("cofoundry-sv"),t=D.model,o=t.visualEditorMode,n=t.isCustomEntityRoute?t.pageRoutingInfo.customEntityRoute:t.pageRoutingInfo.pageRoute;i=e,"SpecificVersion"===o&&t.hasEntityUpdatePermission&&l.addButton({icon:"fa-files-o",title:"Copy to<br />draft",type:"primary",classNames:"publish popup",click:D.copyToDraft}),("Preview"===o||"Edit"===o||"Unpublished"==n.publishStatus&&t.pageVersion.isLatestPublishedVersion&&!t.hasDraftVersion)&&t.hasEntityPublishPermission?l.addButton({icon:"fa-cloud-upload",title:"Publish",type:"primary",classNames:"publish popup",click:D.publish}):"Live"===o&&t.hasEntityPublishPermission&&l.addButton({icon:"fa-cloud-download",title:"Unpublish",type:"primary",classNames:"publish popup",click:D.unpublish})},bindGui:function(){var e,t,o,n,i,s,l,f=document.getElementById("cofoundry-sv__btn-add-block"),d=document.getElementById("cofoundry-sv__block-popover-container"),k=document.getElementsByTagName("body")[0];function c(y){var g=D.model.isCustomEntityRoute?"custom-entity":"page";function e(e,t,o,n){for(var i,s="data-cms-"+g+"-"+e,l=!1,d=y.hasAttribute&&y.hasAttribute(s)?[y]:y.querySelectorAll("["+s+"]"),c=d.length,a=0;a<c;++a){var r,u,p=d[a],m=function(e,t){var o={},n="region-block"===t,i=n?e.parentNode:e;e.offsetWidth||(e.style.width="100%");t=N(e);return o.idName=(o.isBlock?"block":"region")+"_ui_wrap",o.isBlock=n,o.y=t.top,o.x=t.left,o.width=e.offsetWidth,o.height=e.offsetHeight,o.el=e,o.hasContent=n?0<e.innerHTML.length:!e.hasAttribute("data-cms-page-region-empty"),o.html=e.innerHTML,o.name=e.getAttribute(n?"data-cms-page-block-title":"data-cms-page-region-name"),o.regionName=i.getAttribute("data-cms-page-region-name"),o.className=n?"cofoundry-sv__ui-wrap--block":"cofoundry-sv__ui-wrap--region",w(e,o),T(i,o),o}(p,e);m.name&&(l=m.isBlock?Boolean(p.parentNode.getAttribute("data-cms-multi-block")):Boolean(p.getAttribute("data-cms-multi-block")),m.index=a,i=m,r=void 0,(r=wrap_ui_template.cloneNode(!0)).id=i.idName+"_"+i.index,r.className=i.className,r.style.top=i.y+"px",r.style.left=i.x+"px",r.style.width=i.width-2+"px",r.style.height=i.height-2+"px",r.firstChild.nextSibling.innerHTML=i.name,r=r,k.appendChild(r),!l&&(l||m.hasContent)||(u=function(t){var e=f.cloneNode(!0);t.isBlock||t.hasContent?e.style.top=(t.isBlock?t.y+t.height:t.y)+"px":(e.className+=" cofoundry-sv__btn-add-block--empty",e.style.top=t.y+t.height/2+"px");return e.style.left=t.x+t.width/2+"px",e.style.display="block",e.title="Add content block to "+t.regionName,e.addEventListener("click",function(){var e;e="First",b("addRegionBlock",{insertMode:e=t.isBlock?"AfterItem":e,pageTemplateRegionId:t.pageTemplateRegionId,permittedBlockTypes:t.permittedBlockTypes,versionBlockId:t.versionBlockId,pageBlockTypeId:t.pageBlockTypeId,isCustomEntity:D.model.isCustomEntityRoute,regionName:t.regionName,pageId:D.model.page.page.pageId})}),e}(m),k.appendChild(u)),p.addEventListener("mouseenter",t),p.addEventListener("mouseleave",o),n&&p.addEventListener("mousemove",n),current_ui_elements.push({el:p,ui_elements:[r,u],remove:v,events:{mouseenter:t,mouseleave:o,mousemove:n}}))}}e("region",E,h,_),e("region-block",C,I)}function v(){for(var e in this.events)this.events.hasOwnProperty(e)&&this.el.removeEventListener(e,this.events[e]);for(var t=0;t<this.ui_elements.length;t++)this.ui_elements[t]&&k.removeChild(this.ui_elements[t])}function a(){b("moveBlockUp",{versionBlockId:scope.versionBlockId,isCustomEntity:D.model.isCustomEntityRoute,isUp:!0})}function r(){b("moveBlockDown",{versionBlockId:scope.versionBlockId,isCustomEntity:D.model.isCustomEntityRoute,isUp:!1})}function u(){b("editBlock",{versionBlockId:scope.versionBlockId,pageBlockTypeId:scope.pageBlockTypeId,isCustomEntity:D.model.isCustomEntityRoute})}function p(){b("addBlock",{insertMode:"Last",pageTemplateRegionId:scope.pageTemplateRegionId,permittedBlockTypes:scope.permittedBlockTypes,versionBlockId:scope.versionBlockId,pageBlockTypeId:scope.pageBlockTypeId,isCustomEntity:D.model.isCustomEntityRoute,pageId:D.model.page.page.pageId})}function m(){b("addBlockAbove",{insertMode:"BeforeItem",pageTemplateRegionId:scope.pageTemplateRegionId,permittedBlockTypes:scope.permittedBlockTypes,versionBlockId:scope.versionBlockId,pageBlockTypeId:scope.pageBlockTypeId,isCustomEntity:D.model.isCustomEntityRoute,pageId:D.model.page.page.pageId})}function y(){b("addBlockBelow",{insertMode:"AfterItem",pageTemplateRegionId:scope.pageTemplateRegionId,permittedBlockTypes:scope.permittedBlockTypes,versionBlockId:scope.versionBlockId,pageBlockTypeId:scope.pageBlockTypeId,isCustomEntity:D.model.isCustomEntityRoute,pageId:D.model.page.page.pageId})}function g(){b("deleteBlock",{versionBlockId:scope.versionBlockId,isCustomEntity:D.model.isCustomEntityRoute})}function B(e,t,o){e&&(scope.buttons[t]=e).addEventListener("click",o)}function b(e,t){scope.buttons[e],L.contentWindow.postMessage({action:e,args:[t]},document.location.origin),L.style.display="block"}function E(e){R(0,e.target)}function h(){}function I(){}function _(e){R()}function C(e){var o,n;o=e.target,n={},o&&(w(o,scope),A("addBlock",!scope.versionBlockId),A("editBlock",scope.versionBlockId),A("deleteBlock",scope.versionBlockId),A("moveBlockUp",scope.isMultiBlock),A("moveBlockDown",scope.isMultiBlock),A("addBlockAbove",scope.isMultiBlock),A("addBlockBelow",scope.isMultiBlock)),function(){var e=(t=N(o)).top,t=t.left;n={top:e+"px",left:(t||0)+"px"},scope.startScroll=scope.currentScrollY,scope.startY=e,M.style.display="block",M.style.top=n.top,M.style.left=n.left}()}function T(e,t){var o;t.currentElement=e,t.pageTemplateRegionId=e.getAttribute("data-cms-page-template-region-id"),t.permittedBlockTypes=(o=e.getAttribute("data-cms-page-region-permitted-block-types"))?o.split(","):[],t.regionName=e.getAttribute("data-cms-page-region-name"),t.isMultiBlock=e.getAttribute("data-cms-multi-block"),t.isCustomEntity=e.hasAttribute("data-cms-custom-entity-region")}function w(e,t){t.currentBlockElement=e,t.versionBlockId=e.getAttribute("data-cms-version-block-id"),t.pageBlockTypeId=e.getAttribute("data-cms-page-block-type-id")}function R(e,t){t&&(T(t,scope),A("addRegionBlock",scope.isMultiBlock))}function A(e,t){e=scope.buttons[e];e&&(e.style.display=t?"block":"none")}function N(e){if(!e)return{left:0,top:0};var t=e.getBoundingClientRect(),o=void 0!==window.pageXOffset?window.pageXOffset:(document.documentElement||document.body.parentNode||document.body).scrollLeft,e=void 0!==window.pageYOffset?window.pageYOffset:(document.documentElement||document.body.parentNode||document.body).scrollTop;return{top:t.top+e,left:t.left+o}}wrap_ui_template=document.getElementById("cofoundry-sv__ui-wrap"),current_ui_elements=[],timer=null,scope={buttons:{},regionY:-1},M=d,document.getElementsByTagName("html")[0].className=D.model.isCustomEntityRoute?"cofoundry-editmode__custom-entity":"cofoundry-editmode__page","Edit"===D.model.visualEditorMode&&(c(document),e=M.querySelectorAll("#cofoundry-sv__btn-block-moveup")[0],t=M.querySelectorAll("#cofoundry-sv__btn-block-movedown")[0],o=M.querySelectorAll("#cofoundry-sv__btn-block-edit")[0],n=M.querySelectorAll("#cofoundry-sv__btn-block-add")[0],i=M.querySelectorAll("#cofoundry-sv__btn-block-delete")[0],s=M.querySelectorAll("#cofoundry-sv__btn-block-addabove")[0],l=M.querySelectorAll("#cofoundry-sv__btn-block-addbelow")[0],B(e,"moveBlockUp",a),B(t,"moveBlockDown",r),B(o,"editBlock",u),B(n,"addBlock",p),B(s,"addBlockAbove",m),B(l,"addBlockBelow",y),B(i,"deleteBlock",g),window.addEventListener("resize",function(e){M.style.display="none",current_ui_elements.forEach(function(e,t){e.remove()}),current_ui_elements=[],timer&&clearTimeout(timer),timer=setTimeout(function(){c(document)},500)}),document.addEventListener("scroll",function(e){R()}))},publish:function(e){e.preventDefault(),L.contentWindow.postMessage({action:"publish",args:[{entityId:D.model.isCustomEntityRoute?D.model.page.customEntity.customEntityId:D.model.page.page.pageId,isCustomEntity:D.model.isCustomEntityRoute,customEntityDefinition:D.model.customEntityDefinition}]},document.location.origin),L.style.display="block"},unpublish:function(e){e.preventDefault(),L.contentWindow.postMessage({action:"unpublish",args:[{entityId:D.model.isCustomEntityRoute?D.model.page.customEntity.customEntityId:D.model.page.page.pageId,isCustomEntity:D.model.isCustomEntityRoute}]},document.location.origin),L.style.display="block"},copyToDraft:function(e){var t;e.preventDefault(),D.model.isCustomEntityRoute&&(t={entityNameSingular:D.model.customEntityDefinition.nameSingular,isCustomEntity:!0}),L.contentWindow.postMessage({action:"copyToDraft",args:[{entityId:D.model.isCustomEntityRoute?D.model.page.customEntity.customEntityId:D.model.page.page.pageId,versionId:D.model.version.versionId,hasDraftVersion:D.model.hasDraftVersion,dialogOptions:t}]},document.location.origin),L.style.display="block"}},l={addButton:function(e){var t=e.type||"secondary",o=document.createElement("a"),n=document.createElement("span");o.appendChild(n),o.className=("primary"===t?"cofoundry-sv__options__button":"cofoundry-sv__mode__button")+" "+e.classNames,n.innerHTML=e.title,e.click?(o.href="#",o.addEventListener("click",e.click,!1)):o.href=e.href||"#",(s=i.getElementsByClassName("primary"===t?"cofoundry-sv__options":"cofoundry-sv__mode")[0])&&s.appendChild(o)}};return window.addEventListener("load",function(){D.model=Cofoundry.PageResponseData,null!=D.model&&(D.bindIframe(),D.bindGui(),D.bindToolbar())}),{toolBar:l}}();