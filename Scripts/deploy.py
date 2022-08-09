import os
import sys
import xml.etree.ElementTree as ElementTree
from pathlib import Path

scriptPath: str = os.path.dirname(os.path.abspath(__file__))
rootPath: str = Path(scriptPath).parent.absolute()

config = ElementTree.parse(f"{rootPath}/Configs/deploy.xml")

serviceName = config.findtext("service")
databaseHost = config.findtext("database_host")
databasePort = config.findtext("database_port")
databaseName = config.findtext("database")
databaseUser = config.findtext("database_user")
databasePassword = config.findtext("database_password")
imageRoot = config.findtext("image_root")
aspEnvironment = config.findtext("asp_environment")
user = config.findtext("user")
url = config.findtext("url")
urlAdmin = config.findtext("url_admin")
urlApi = config.findtext("url_api")


def format_service_line(line: str) -> str:
    return line\
        .replace("NAME", serviceName)\
        .replace("EXECFILE", f"{rootPath}/WebAPI/bin/Service/netcoreapp3.1/WebAPI")\
        .replace("USER", user)\
        .replace("ASPENVIRONMENT", aspEnvironment)\
        .replace("WORKINGDIRECTORY", f"{rootPath}/WebAPI/bin/Service/netcoreapp3.1")


def format_appsettings_line(line: str) -> str:
    return line \
        .replace("USER", databaseUser) \
        .replace("DATABASE", databaseName) \
        .replace("PASSWORD", databasePassword) \
        .replace("HOST", databaseHost) \
        .replace("PORT", databasePort) \
        .replace("IMAGEROOT", imageRoot) \
        .replace("APIORIGIN", urlApi) \
        .replace("ADMINORIGIN", urlAdmin) \
        .replace("ORIGIN", url) \
        .replace("TEMPLATESROOT", config.findtext("templates_root")) \
        .replace("EMAIL_ENABLE", config.findtext("email_enable")) \
        .replace("EMAIL_REDIRECT", config.findtext("email_redirect")) \
        .replace("EMAIL_REDIRECTADDRESS", config.findtext("email_redirect_address")) \
        .replace("EMAIL_HOST", config.findtext("email_host")) \
        .replace("EMAIL_USER", config.findtext("email_user")) \
        .replace("EMAIL_PASSWORD", config.findtext("email_password")) \
        .replace("EMAIL_FROMADDRESS", config.findtext("email_from_address")) \
        .replace("EMAIL_NAME", config.findtext("email_name"))


def create_service_file():
    create_file(f"{rootPath}/Services/sample.service",
                f"{rootPath}/Services/{serviceName}.service",
                format_service_line)


def create_appsettings_file():
    create_file(f"{rootPath}/WebAPI/appsettings.sample.json",
                f"{rootPath}/WebAPI/appsettings.{aspEnvironment}.json",
                format_appsettings_line)


def create_file(input_path: str, output_path: str, format_function):
    with open(input_path) as inputFile:
        with open(output_path, 'w+') as outputFile:
            for line in inputFile:
                outputFile.write(format_function(line))


def clear():
    os.system(f"systemctl stop {serviceName}")
    os.system(f"rm -r {rootPath}/WebAPI/bin/Debug")
    os.system(f"rm -r {rootPath}/WebAPI/bin/Service")


def refresh_database(with_data: bool):
    os.system(f"dropdb {databaseName}")
    os.system(f"createdb {databaseName}")
    os.system(f"psql -f {rootPath}/Database/database.sql {databaseName}")
    if with_data:
        os.system(f"psql -f {rootPath}/Database/testData.sql {databaseName}")
        images()
    else:
        os.system(f"psql -f {rootPath}/Database/staticData.sql {databaseName}")
    os.system(f"psql -f {rootPath}/Database/grant.sql {databaseName}")


def build():
    os.chdir(f"{rootPath}/WebAPI")
    os.system("dotnet clean")
    os.system("dotnet build")
    os.system("cp -r ./bin/Debug ./bin/Service")


def service():
    os.system(f"cp {rootPath}/Services/{serviceName}.service /etc/systemd/system/{serviceName}.service")
    os.system("systemctl daemon-reload")
    os.system(f"systemctl start {serviceName}")


def images():
    regex: str = r"/.*\.(gif|jpe?g|bmp|png|svg)$/igm"
    os.system(f"find {imageRoot}/ -name '{regex}' -delete")
    os.system(f"cp -a {rootPath}/TestData/UploadedImages/. {imageRoot}")
    os.system(f"chown -R {user} {imageRoot}")


def static_images():
    os.system(f"cp -a {rootPath}/StaticImages/. {imageRoot}")


def has_argument(character: str) -> bool:
    for arg in sys.argv:
        if arg[0] == '-':
            if character in arg:
                return True
    return False


clear()
if has_argument("d"):
    refresh_database(has_argument("t"))
    static_images()
create_appsettings_file()
build()
create_service_file()
service()
