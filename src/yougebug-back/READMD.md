因为前端使用的是 Anuglar
所以要做配置，以允许浏览器使用深链接导航

IIS：
在 web.config 下添加
```
<rewrite>
  <rules>
    <rule name="HTTPS force" enabled="true" stopProcessing="true">
      <match url="(.*)"/>
      <conditions>
        <add input="{HTTPS}" pattern="^OFF$"/>
      </conditions>
      <action type="Redirect" url="https://{HTTP_HOST}/{R:1}" redirectType="Permanent"/>
    </rule>
    <rule name="Angular" stopProcessing="true">
      <match url=".*" />
      <conditions logicalGrouping="MatchAll">
        <add input="{REQUEST_FILENAME}" matchType="IsFile" negate="true" />
        <add input="{REQUEST_FILENAME}" matchType="IsDirectory" negate="true" />
      </conditions>
      <action type="Rewrite" url="/" />
    </rule>
  </rules>
</rewrite>
```