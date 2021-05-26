#pip install you-get
#下载：you-get -o F:/temp --playlist --format=flv720 https://www.bilibili.com/video/BV1Fc411h7G3
#下载：you-get --playlist --format=flv720 https://www.bilibili.com/video/BV1Fc411h7G3
#  注：不用-o路径也可以，默认是在当前文件夹
#查询：you-get -i https://www.bilibili.com/video/BV1Fc411h7G3


#import requests

#def DownloadVideo(links):
#    root='F:/下载/谷歌浏览器下载/tmp/'
#    for link in links:
#        file_name = link.split('/')[-1]
#        print("文件下载:%s" % file_name)
#        r = requests.get(link, stream=True)
#        with open(root+file_name, 'wb') as f:
#            for chunk in r.iter_content(chunk_size = 1920 * 1080):
#                if chunk:
#                    f.write(chunk)

#        print("%s 下载完成!\n" % file_name)
#    print("所有视频下载完成!")
#    return

#if __name__ == "__main__":
#    links = ["https://www.bilibili.com/video/BV1Fc411h7G3"]
#    DownloadVideo(links)